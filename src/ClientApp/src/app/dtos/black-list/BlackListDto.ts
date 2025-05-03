export interface BlackListDto {
  id: string;
  blockerProfileId: number;
  blockedProfileId: number;
  blockedProfileFullName: string;
  blockedProfileMainImageUrl: string;
  createdAt: string;
}
