import {Injectable} from "@angular/core";
import {HttpClient, HttpContext, HttpHeaders} from "@angular/common/http";
import {BehaviorSubject, Observable} from "rxjs";
import {
  blackListsEndpoints,
  chatsEndpoints,
  likesEndpoints,
  matchesEndpoints, messagesEndpoints, notificationsEndpoints,
  profilesEndpoints,
} from "../constants/api-endpoints";
import {map, retry, tap} from "rxjs/operators";
import {AddLikeDto} from "../dtos/like/AddLikeDto";
import {LikeDto} from "../dtos/like/LikeDto";
import {ProfileDto} from "../dtos/profile/ProfileDto";
import {UpdateLocationDto} from "../dtos/match/UpdateLocationDto";
import {ChatDto} from "../dtos/chat/ChatDto";
import {MessageDto} from "../dtos/message/MessageDto";
import {CreateChatDto} from "../dtos/chat/CreateChatDto";
import {_IGNORED_STATUSES} from "../constants/http-context";
import {GeneralResponseDto} from "../dtos/shared/generalResponseDto";
import {NotificationDto} from "../dtos/notification/NotificationDto";
import {CreateBlackListDto} from "../dtos/black-list/CreateBlackListDto";
import {RemoveFromBlackListDto} from "../dtos/black-list/RemoveFromBlackListDto";
import {BlackListDto} from "../dtos/black-list/BlackListDto";

@Injectable({
  providedIn: 'root'
})
export class MatchService {
  private httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  private recsLoadingSubject = new BehaviorSubject<boolean>(true);
  recsLoading$ = this.recsLoadingSubject.asObservable();

  constructor(private httpClient: HttpClient) { }

  getRecs(): Observable<ProfileDto[]> {
    this.recsLoadingSubject.next(true);

    return this.httpClient.get<ProfileDto[]>(`${profilesEndpoints.recs()}`,
      this.httpOptions).pipe(
      tap(() => {
        this.recsLoadingSubject.next(false);
      })
    );
  }

  addLike(model: AddLikeDto){
    return this.httpClient.post<LikeDto>(`${likesEndpoints.likes}`, model, this.httpOptions)
      .pipe(
      );
  }

  updateLocation(model: UpdateLocationDto){
    return this.httpClient.patch(`${profilesEndpoints.location}`, model, this.httpOptions)
      .pipe(
      );
  }

  getPaginatedMatches(pageSize: number, pageNumber: number): Observable<{ matches: ProfileDto[], pagination: any }> {
    return this.httpClient.get<ProfileDto[]>(`${matchesEndpoints.paged(pageSize.toString(), pageNumber.toString())}`,
      {
        ...this.httpOptions,
        observe: 'response'
      }).pipe(
      map(response => {
        const pagination = JSON.parse(response.headers.get('X-Pagination')!);
        return {
          matches: response.body || [],
          pagination: pagination
        };
      }),
    );
  }

  getPaginatedChats(pageSize: number, pageNumber: number): Observable<{ chats: ChatDto[], pagination: any }> {
    return this.httpClient.get<ChatDto[]>(`${chatsEndpoints.paged(pageSize.toString(), pageNumber.toString())}`,
      {
        ...this.httpOptions,
        observe: 'response'
      }).pipe(
      map(response => {
        const pagination = JSON.parse(response.headers.get('X-Pagination')!);

        return {
          chats: response.body || [],
          pagination: pagination
        };
      }),
    );
  }

  getPaginatedMessages(pageSize: number, pageNumber: number, chatId: string): Observable<{ messages: MessageDto[], pagination: any }> {
    return this.httpClient.get<MessageDto[]>(`${messagesEndpoints.paged(pageSize.toString(), pageNumber.toString(), chatId)}`,
      {
        ...this.httpOptions,
        observe: 'response'
      }).pipe(
      map(response => {
        const pagination = JSON.parse(response.headers.get('X-Pagination')!);
        return {
          messages: response.body || [],
          pagination: pagination
        };
      }),
    );
  }

  getChatByProfilesIds(firstProfileId: number, secondProfileId: number){
    return this.httpClient.get<ChatDto>(chatsEndpoints.chatsByIds(firstProfileId, secondProfileId), {
      headers: this.httpOptions.headers,
      context: new HttpContext().set(_IGNORED_STATUSES, true),
    })
      .pipe(
      );
  }

  createChat(model: CreateChatDto){
    return this.httpClient.post<ChatDto>(chatsEndpoints.chats, model, this.httpOptions)
      .pipe(
      );
  }

  deleteChatById(chatId: string){
    return this.httpClient.delete<GeneralResponseDto>(`${chatsEndpoints.chats}/${chatId}`, this.httpOptions)
      .pipe(
      );
  }

  readChat(chatId: string, profileId: number) {
    return this.httpClient.post(`${chatsEndpoints.chats}/read`, {
      chatId: chatId,
      profileId: profileId
    }, this.httpOptions);
  }

  getNotifications(): Observable<NotificationDto[]> {
    return this.httpClient.get<NotificationDto[]>(`${notificationsEndpoints.notifications}`, this.httpOptions)
      .pipe(
      );
  }

  markNotificationsAsRead(notificationIds: string[]): Observable<void> {
    return this.httpClient.post<void>(`${notificationsEndpoints.notifications}/read`, {notificationIds}, this.httpOptions)
      .pipe(
      );
  }

  addToBlackList(model: CreateBlackListDto): Observable<void> {
    return this.httpClient.post<void>(blackListsEndpoints.blackLists, model, this.httpOptions);
  }

  getBlackList(): Observable<BlackListDto[]> {
    return this.httpClient.get<BlackListDto[]>(blackListsEndpoints.blackLists, this.httpOptions);
  }

  removeFromBlackList(model: RemoveFromBlackListDto): Observable<void> {
    return this.httpClient.delete<void>(blackListsEndpoints.blackLists, {
      ...this.httpOptions,
      body: model
    });
  }

  getCompatibility(birthDate1: Date, birthDate2: Date) {
    return this.httpClient.post<any>(`${matchesEndpoints.matches}/compatibility`, {
      birthDate1: birthDate1,
      birthDate2: birthDate2
    }, this.httpOptions);
  }
}
