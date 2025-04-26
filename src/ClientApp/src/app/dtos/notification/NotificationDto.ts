export interface NotificationDto {
  id: string;
  profileId: number;
  type: string;
  title: string;
  body: string;
  isRead: boolean;
  chatId : string | null;
  senderId : number;
  createdAt: string;
}
