import {
  Component,
  OnInit,
  Input,
  OnDestroy,
  ViewChild,
  ElementRef,
  OnChanges,
  SimpleChanges,
  EventEmitter, Output
} from '@angular/core';
import { MatchService } from "../../../services/match-service.service";
import { MessageDto } from "../../../dtos/message/MessageDto";
import {DatePipe, NgClass, NgForOf, NgIf} from "@angular/common";
import { InfiniteScrollDirective } from "ngx-infinite-scroll";
import { ChatSignalRService } from "../../../services/chat-signalr.service";
import {ChatDto} from "../../../dtos/chat/ChatDto";
import {Subscription} from "rxjs";

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css'],
  imports: [
    DatePipe,
    NgClass,
    NgForOf,
    InfiniteScrollDirective,
    NgIf
  ],
  standalone: true
})
export class ChatMessagesComponent implements OnInit, OnDestroy, OnChanges {
  @Input() chatId!: string;
  @Input() selectedChat!: ChatDto;
  @Input() profileId!: number;

  @ViewChild('chatContainer', { static: true }) chatContainer!: ElementRef;

  messages: MessageDto[] = [];
  pageNumber: number = 1;
  pageSize: number = 12;
  isLoading: boolean = false;
  pagination: any = {};
  messageStatuses: Record<string, 'read' | 'unread'> = {};
  private subscriptions: Subscription[] = [];
  @Output() chatRead = new EventEmitter<void>();

  private isConnected: boolean = false;

  constructor(
    private matchService: MatchService,
    private chatSignalRService: ChatSignalRService
  ) {}

  ngOnInit(): void {
    this.subscriptions.push(
      this.chatSignalRService.isConnected$.subscribe(isConnected => {
        this.isConnected = isConnected;
        if (isConnected) {
          this.chatSignalRService.joinChat(this.chatId, String(this.profileId));
        }
      })
    );

    this.subscriptions.push(
      this.chatSignalRService.newMessage$.subscribe(message => {
        if (message && message.chatId === this.chatId) {
          const exists = this.messages.some(m => m.id === message.id);
          if (!exists) {
            this.messages.push(message);
            this.updateMessagesStatus();
            setTimeout(() => this.scrollToBottom(), 100);
          }
        }
      })
    );
    this.chatSignalRService.onMessagesRead((chatId, profileId) => {
      if (chatId === this.chatId) {
        this.updateMessagesStatus();
      }
    });
  }

  ngOnDestroy(): void {
    if (this.chatId && this.profileId) {
      this.chatSignalRService.leaveChat(this.chatId, String(this.profileId));
    }
    this.subscriptions.forEach(sub => sub.unsubscribe());
    //this.chatSignalRService.stopConnection();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['chatId'] && changes['chatId'].currentValue) {
      this.messages = [];
      this.pageNumber = 1;
      this.loadMessages();
      this.markChatAsRead();
      setTimeout(() => this.scrollToBottom(), 100);
    }
  }

  loadMessages(): void {
    this.isLoading = true;
    this.matchService.getPaginatedMessages(this.pageSize, this.pageNumber, this.chatId).subscribe({
      next: (response) => {
        const newMessages = response.messages.reverse();

        const filteredMessages = newMessages.filter(newMsg =>
          !this.messages.some(existing => existing.id === newMsg.id)
        );

        this.messages = [...filteredMessages, ...this.messages];
        this.pagination = response.pagination;
        this.updateMessagesStatus();
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
      this.chatSignalRService.sendMessage(this.chatId, String(this.profileId), content).then(() => {
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
    if (this.chatId && this.profileId) {
      this.chatSignalRService.readChat(this.chatId, String(this.profileId));
      this.chatRead.emit();
    }
  }

  getMessageStatus(message: MessageDto, index: number): 'read' | 'unread' | null {
    if (!this.selectedChat) return null;

    if (!this.isSender(message)) {
      return null;
    }

    const myMessages = this.messages.filter(m => this.isSender(m));
    const myMessageIndex = myMessages.indexOf(message);

    const receiverUnreadCount = this.selectedChat.receiverProfileUnreadCount ?? 0;

    if (myMessageIndex >= (myMessages.length - receiverUnreadCount)) {
      return 'unread';
    } else {
      return 'read';
    }
  }

  updateMessagesStatus(): void {
    if (this.selectedChat && this.selectedChat.receiverProfileUnreadCount !== undefined) {
      this.messages.forEach((message, index) => {
        const status = this.getMessageStatus(message, index);
        if(status)
        this.messageStatuses[message.id] = status;
      });
    }
  }

  get isBlocked(): boolean {
    return !!this.selectedChat?.isBlockedMessage;
  }
}
