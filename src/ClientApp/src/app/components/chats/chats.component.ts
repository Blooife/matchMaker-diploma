import {Component, OnInit, HostListener, OnDestroy, ChangeDetectorRef} from '@angular/core';
import {ChatDto} from "../../dtos/chat/ChatDto";
import {MatchService} from "../../services/match-service.service";
import {DatePipe, NgClass, NgForOf, NgIf, NgStyle} from "@angular/common";
import {ChatMessagesComponent} from "./messages/messages.component";
import {FormsModule} from "@angular/forms";
import {ProfileService} from "../../services/profile-service.service";
import {Observable, of, Subscription} from "rxjs";
import {ProfileDto} from "../../dtos/profile/ProfileDto";
import {ActivatedRoute, Router} from "@angular/router";
import {CreateChatDto} from "../../dtos/chat/CreateChatDto";
import {InfiniteScrollDirective} from "ngx-infinite-scroll";
import {ProfileCardComponent} from "../profile/profile-card/profile-card.component";
import {catchError, map, switchMap} from "rxjs/operators";
import {ChatSignalRService} from "../../services/chat-signalr.service";
import {CreateReportComponent} from "../create-report/create-report.component";

@Component({
  selector: 'app-chats',
  templateUrl: './chats.component.html',
  styleUrls: ['./chats.component.css'],
    imports: [
        NgForOf,
        ChatMessagesComponent,
        NgIf,
        FormsModule,
        DatePipe,
        NgClass,
        InfiniteScrollDirective,
        ProfileCardComponent,
        NgStyle,
        CreateReportComponent,
    ],
  standalone: true
})
export class ChatsComponent implements OnInit, OnDestroy {
  chats: ChatDto[] = [];
  pageNumber: number = 1;
  pageSize: number = 12;
  profileId: number | undefined;
  isLoading: boolean = false;
  pagination: any = {};
  selectedChat?: ChatDto;
  profile: ProfileDto | null = null;
  selectedProfile: ProfileDto | null = null;
  showContextMenu = false;
  contextMenuPosition = { x: 0, y: 0 };
  selectedChatId?: string;
  private subscriptions: Subscription[] = [];
  reportProfileId: number | null = null;
  reportModalVisible: boolean = false;

  constructor(private matchService: MatchService,
              private profileService:ProfileService,
              private chatSignalRService: ChatSignalRService,
              private route: ActivatedRoute) {

  }

  ngOnInit(): void {
    const sub = this.profileService.profile$.subscribe(
      (profileDto) =>{
        this.profile = profileDto;
        this.profileId = profileDto!.id;

        const token = localStorage.getItem('access_token');
        console.log("ddd"+token)
        if (token) {
          this.chatSignalRService.startConnection(token);

          this.chatSignalRService.onUnreadCountUpdated((chatId, requestedProfileUnreadCount, receiverProfileUnreadCount) => {
            const chat = this.chats.find(c => c.id === chatId);
            if (chat) {
              chat.requestedProfileUnreadCount = requestedProfileUnreadCount;
              chat.receiverProfileUnreadCount = receiverProfileUnreadCount;
              console.log(requestedProfileUnreadCount)
              console.log(receiverProfileUnreadCount)
              if (this.selectedChat && this.selectedChat.id === chatId) {
                this.selectedChat.requestedProfileUnreadCount = requestedProfileUnreadCount;
                this.selectedChat.receiverProfileUnreadCount = receiverProfileUnreadCount;
              }
            }
          });

          this.chatSignalRService.onMessagesRead((chatId, profileId) => {
            const chat = this.chats.find(c => c.id === chatId);
            if (chat) {
              if(chat.firstProfileId == profileId){
                chat.requestedProfileUnreadCount = 0;
              }else{
                chat.receiverProfileUnreadCount = 0;
              }
              if (this.selectedChat && this.selectedChat.id === chatId) {
                if(this.selectedChat.firstProfileId == profileId) {
                  this.selectedChat.requestedProfileUnreadCount = 0;
                }else{
                  this.selectedChat.receiverProfileUnreadCount = 0;
                }
              }
            }
          });
        }
      }
    )
    this.subscriptions.push(sub);

    this.route.queryParams.pipe(
      switchMap(params => {
        const firstProfileId = params['firstProfileId'];
        const secondProfileId = params['secondProfileId'];
        if (firstProfileId && secondProfileId) {
          return this.startOrCreateChat(firstProfileId, secondProfileId);
        }
        return of(null);
      })
    ).subscribe(() => {
      this.loadChats();
    });
  }

  ngOnDestroy() {
    this.subscriptions.forEach(sub=>sub.unsubscribe());
    this.leaveChat();
  }

  loadChats(): void {
    this.isLoading = true;
    this.matchService.getPaginatedChats(this.pageSize, this.pageNumber, this.profile!.id).subscribe({
      next: (response) => {
        this.chats.push(...response.chats);
        this.pagination = response.pagination;
      },
      error: () => {
        this.isLoading = false;
      },
      complete:() => {
        this.isLoading = false;
      }
    });
  }

  openChat(chat: ChatDto): void {
    this.closeModal();
    this.selectedChat = chat;
    if (chat.requestedProfileUnreadCount > 0) {
      chat.receiverProfileUnreadCount = 0;
    }
  }

  closeModal(): void {
    this.selectedChat = undefined;
    this.selectedProfile = null;
    this.subscriptions.forEach(sub=>sub.unsubscribe());
  }

  startOrCreateChat(firstProfileId: number, secondProfileId: number): Observable<any> {
    return this.matchService.getChatByProfilesIds(firstProfileId, secondProfileId).pipe(
      map(chat => {
        this.selectedChat = chat;
        return chat;
      }),
      catchError(() => {
        const createChatDto: CreateChatDto = {
          firstProfileId: firstProfileId,
          secondProfileId: secondProfileId,
        };
        return this.matchService.createChat(createChatDto).pipe(
          map(newChat => {
            this.selectedChat = newChat;
            return newChat;
          })
        );
      })
    );
  }

  onScrollDown(): void {
    if (!this.isLoading && this.pagination.HasNext) {
      this.pageNumber++;
      this.loadChats();
    }
  }

  viewProfile(profileId: number): void {
    this.profileService.getProfileById(profileId).subscribe({
        next: (profile) => {
          this.selectedProfile = profile
        }
      }
    )
  }

  closeProfileModal(){
    this.selectedProfile = null;
  }

  deleteChat(chatId: string){
    this.matchService.deleteChatById(chatId).subscribe(
      () => {
        this.chats = this.chats.filter(chat => chat.id !== chatId);
        this.closeModal();
      }
    )
  }

  addToBlackList(chatId: string): void {
    const chat = this.chats.find(c => c.id === chatId);
    if (chat) {
      const blockedProfileId = chat.firstProfileId == this.profileId ? chat.secondProfileId : chat.firstProfileId;
      this.matchService.addToBlackList({ blockedProfileId: blockedProfileId })
        .subscribe(() => {
        });
    }
  }

  onRightClick(event: MouseEvent, chat: ChatDto): void {
    event.preventDefault();
    this.selectedChatId = chat.id;
    this.contextMenuPosition = { x: event.clientX, y: event.clientY };
    this.showContextMenu = true;
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent): void {
    this.showContextMenu = false;
  }

  leaveChat(): void {
    if (this.selectedChat && this.profile) {
      this.chatSignalRService.leaveChat(this.selectedChat.id, String(this.profile.id));
      this.closeModal();
    }
  }

  refreshChats(){
    this.matchService.getPaginatedChats(this.pageSize, this.pageNumber, this.profile!.id).subscribe({
      next: (response) => {
        this.chats = response.chats;
        this.pagination = response.pagination;
      },
      error: () => {
        this.isLoading = false;
      },
      complete:() => {
        this.isLoading = false;
      }
    });
  }

  openReportModal(profileId: number): void {
    this.reportProfileId = profileId;
    this.reportModalVisible = true;
  }

  closeReportModal(): void {
    this.reportModalVisible = false;
  }
}
