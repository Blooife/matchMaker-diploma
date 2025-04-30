export interface ChatDto{
  id: string;
  profileName: string;
  profileLastName: string;
  firstProfileId: number;
  secondProfileId: number;
  mainImageUrl: string;
  requestedProfileUnreadCount: number;
  receiverProfileUnreadCount: number;
}
