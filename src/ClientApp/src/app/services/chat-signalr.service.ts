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
      .withUrl('https://localhost:5000/chat', {
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

  onUnreadCountUpdated(callback: (chatId: string, requestedProfileUnreadCount: number, receiverProfileUnreadCount: number) => void): void {
    this.hubConnection?.on('UpdateUnreadCount', (chatId: string, requestedProfileUnreadCount: any, receiverProfileUnreadCount: any) => {
      console.log("upd unr count")
      callback(chatId, Number(requestedProfileUnreadCount), Number(receiverProfileUnreadCount));
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

  leaveChat(chatId: string, profileId: string): void {
    if (this.hubConnection) {
      this.hubConnection.invoke('LeaveChat', chatId, Number(profileId)).then(() => {
        this.currentChatId = null;
        console.log(`Покинули чат: ${chatId}`);
      }).catch(err => {
        console.error('LeaveChat Error: ', err);
      });
    }
  }

  readChat(chatId: string, profileId: string): void {
    this.hubConnection?.invoke('ReadChat', chatId, profileId)
      .catch(err => console.error('Failed to mark chat as read via SignalR', err));
  }

  onMessagesRead(callback: (chatId: string, profileId: number) => void): void {
    this.hubConnection?.on('MessagesRead', (chatId: string, profileId: number) => {
      callback(chatId, profileId);
    });
  }
}
