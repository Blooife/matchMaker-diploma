import {Component, OnInit, Input, OnDestroy, ViewChild, ElementRef, OnChanges, SimpleChanges} from '@angular/core';
import { MatchService } from "../../../services/match-service.service";
import { MessageDto } from "../../../dtos/message/MessageDto";
import { DatePipe, NgClass, NgForOf } from "@angular/common";
import * as SignalR from '@microsoft/signalr';
import {InfiniteScrollDirective} from "ngx-infinite-scroll";

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css'],
  imports: [
    DatePipe,
    NgClass,
    NgForOf,
    InfiniteScrollDirective
  ],
  standalone: true
})
export class ChatMessagesComponent implements OnDestroy, OnChanges {
  @Input() chatId!: string;
  @Input() profileId!: string;

  @ViewChild('chatContainer', { static: true }) chatContainer!: ElementRef;

  messages: MessageDto[] = [];
  pageNumber: number = 1;
  pageSize: number = 12;
  isLoading: boolean = false;
  pagination: any = {};
  private hubConnection!: SignalR.HubConnection;
  private isConnectionActive = false;

  constructor(private matchService: MatchService) {}

  ngOnDestroy(): void {
    this.hubConnection?.stop();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['chatId'] && changes['chatId'].currentValue) {
      this.updateConnection();
      this.messages = [];
      this.pageNumber = 1
      this.loadMessages();
      setTimeout(() => {
        this.scrollToBottom();
      }, 100);
    }
  }

  loadMessages(): void {
    this.isLoading = true;
    this.matchService.getPaginatedMessages(this.pageSize, this.pageNumber, this.chatId).subscribe({
      next: (response) => {
        const mes = response.messages.reverse();
        this.messages = [...mes, ...this.messages];
        this.pagination = response.pagination;
      },
      error: () => {
        this.isLoading = false;
      },
      complete: () =>{
        this.isLoading = false;
      }
    });
  }

  onScrollUp(): void {
    if (!this.isLoading && this.pagination.HasNext) {
      this.pageNumber++;
      this.loadMessages();
    }
  }

  isSender(message: MessageDto): boolean {
    return message.senderId === this.profileId;
  }

  initializeSignalR(): void {
    this.hubConnection = new SignalR.HubConnectionBuilder()
      .withUrl('https://localhost:5003/chat')
      .build();

    this.hubConnection.on('ReceiveMessage', (message: MessageDto) => {
      if (message.chatId === this.chatId) {
        this.messages.push(message);
        this.scrollToBottom();
      }
    });

    this.hubConnection
      .start()
      .then(() => {
        console.log('SignalR Connected');
        this.hubConnection.invoke('JoinChat', this.chatId);
        this.isConnectionActive = true;
      })
      .catch(err => console.error('SignalR Connection Error: ', err));
  }

  sendMessage(content: string): void {
        this.hubConnection.invoke('SendMessage', this.chatId, this.profileId, content)
      .then(() => {
        setTimeout(() => {
          this.scrollToBottom();
        }, 100);
      })
      .catch(err => console.error('Send Message Error: ', err));
  }

  scrollToBottom(): void {
    this.chatContainer.nativeElement.scrollTop = this.chatContainer.nativeElement.scrollHeight;
  }

  private updateConnection(): void {
    if (this.isConnectionActive) {
      this.hubConnection.stop().then(() => {
        this.initializeSignalR();
      });
    } else {
      this.initializeSignalR();
    }
  }
}
