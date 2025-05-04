export interface UserReportDto {
  id: number;
  reporterUserId: number;
  reportedUserId: number;
  reason: string;
  createdAt: string;

  reporterEmail: string;
  reportedEmail: string;

  moderatorEmail?: string | null;

  status: string;
  reportTypeId: number;
  reportType: string;

  moderatorComment?: string | null;
  reviewedByModeratorId?: number | null;
  reviewedAt?: string | null;
}
