import { Injectable } from '@angular/core';
import * as SignalR from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';
import {MessageDto} from "../dtos/message/MessageDto";

@Injectable({
  providedIn: 'root'
})
export class ChatSignalRService {
  private hubConnection: SignalR.HubConnection | null = null;
  private isConnectedSubject = new BehaviorSubject<boolean>(false);
  private currentChatId: string | null = null;
  private newMessageSubject = new BehaviorSubject<MessageDto | null>(null);
  public newMessage$ = this.newMessageSubject.asObservable();


  constructor() {}

  startConnection(token: string): void {
    if (this.hubConnection) {
      return;
    }

    this.hubConnection = new SignalR.HubConnectionBuilder()
      .withUrl('https://localhost:5003/chat', {
        accessTokenFactory: () => token
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.on('ReceiveMessage', (message: any) => {
      if (this.currentChatId && message.chatId === this.currentChatId) {
        console.log('Новое сообщение:', message);
        this.newMessageSubject.next(message);
      }
    });

    this.hubConnection
      .start()
      .then(() => {
        console.log('SignalR Connected');
        this.isConnectedSubject.next(true);
      })
      .catch(err => {
        console.error('SignalR Connection Error: ', err);
      });
  }

  joinChat(chatId: string, profileId: string): void {
    if (this.hubConnection) {
      this.hubConnection.invoke('JoinChat', chatId, Number(profileId)).then(() => {
        this.currentChatId = chatId;
        console.log(`Присоединились к чату: ${chatId}`);
      }).catch(err => console.error('JoinChat Error: ', err));
    }
  }

  sendMessage(chatId: string, profileId: string, message: string): Promise<void> {
    if (this.hubConnection) {
      return this.hubConnection.invoke('SendMessage', chatId, Number(profileId), message)
        .then(() => {
          console.log('Сообщение отправлено');
        })
        .catch(err => {
          console.error('Send Message Error: ', err);
          throw err; // Чтобы ошибка была выброшена и её можно было поймать в компоненте
        });
    } else {
      return Promise.reject('Нет соединения с сервером');
    }
  }


  onUnreadCountUpdated(callback: (chatId: string, unreadCount: number) => void): void {
    this.hubConnection?.on('UpdateUnreadCount', (chatId: string, unreadCount: number) => {
      callback(chatId, unreadCount);
    });
  }

  stopConnection(): void {
    if (this.hubConnection) {
      this.hubConnection.stop();
      this.hubConnection = null;
      this.isConnectedSubject.next(false);
    }
  }

  get isConnected$() {
    return this.isConnectedSubject.asObservable();
  }
}
