import { ReportStatus } from "../../constants/report-status";

export interface ModerateReportDto {
  reportId: number;
  status: ReportStatus;
  moderatorComment?: string | null;
  banUntil?: string | null;
}
