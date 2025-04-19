import { Component, OnInit, Input, OnDestroy, ViewChild, ElementRef, OnChanges, SimpleChanges } from '@angular/core';
import { MatchService } from "../../../services/match-service.service";
import { MessageDto } from "../../../dtos/message/MessageDto";
import { DatePipe, NgClass, NgForOf } from "@angular/common";
import { InfiniteScrollDirective } from "ngx-infinite-scroll";
import { ChatSignalRService } from "../../../services/chat-signalr.service";

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
export class ChatMessagesComponent implements OnInit, OnDestroy, OnChanges {
  @Input() chatId!: string;
  @Input() profileId!: string;

  @ViewChild('chatContainer', { static: true }) chatContainer!: ElementRef;

  messages: MessageDto[] = [];
  pageNumber: number = 1;
  pageSize: number = 12;
  isLoading: boolean = false;
  pagination: any = {};

  private isConnected: boolean = false;

  constructor(
    private matchService: MatchService,
    private chatSignalRService: ChatSignalRService
  ) {}

  ngOnInit(): void {
    this.chatSignalRService.isConnected$.subscribe(isConnected => {
      this.isConnected = isConnected;
      console.log(isConnected)
      if (isConnected) {
        this.chatSignalRService.joinChat(this.chatId, this.profileId);
      }
    });

    this.chatSignalRService.newMessage$.subscribe(message => {
      if (message && message.chatId === this.chatId) {
        this.messages.push(message);
        setTimeout(() => this.scrollToBottom(), 100);
      }
    });
  }

  ngOnDestroy(): void {
    // Закрываем соединение при уничтожении компонента
    this.chatSignalRService.stopConnection();
  }

  ngOnChanges(changes: SimpleChanges): void {
    // Если chatId изменился, загружаем сообщения для нового чата
    if (changes['chatId'] && changes['chatId'].currentValue) {
      this.messages = []; // очищаем сообщения
      this.pageNumber = 1;
      this.loadMessages();
      this.markChatAsRead();
      setTimeout(() => this.scrollToBottom(), 100); // прокручиваем чат вниз
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
      complete: () => {
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
    return message.senderId == this.profileId;
  }

  sendMessage(content: string): void {
    if (this.isConnected) {
      this.chatSignalRService.sendMessage(this.chatId, this.profileId, content).then(() => {
        setTimeout(() => {
          this.scrollToBottom();
        }, 100);
      });
    } else {
      console.error('Нет соединения с сервером');
    }
  }

  scrollToBottom(): void {
    this.chatContainer.nativeElement.scrollTop = this.chatContainer.nativeElement.scrollHeight;
  }

  markChatAsRead(): void {
    this.matchService.readChat(this.chatId, this.profileId).subscribe({
      next: () => {},
      error: (err) => console.error('Mark chat as read failed', err)
    });
  }
}
