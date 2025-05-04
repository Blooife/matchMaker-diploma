import {ReportStatus} from "../../constants/report-status";

export interface ReportFilterDto {
  reporterUserEmail?: string;
  reportedUserEmail?: string;
  reportTypeId?: number;
  status?: ReportStatus;
  createdFrom?: string | null;
  createdTo?: string | null;
  notReviewed?: boolean;
}
