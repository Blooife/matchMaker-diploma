import { Component, OnDestroy, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
import {ReportStatus, StatusTranslatePipe} from "../../constants/report-status";
import {ReportFilterDto} from "../../dtos/report/ReportFilterDto";
import {UserReportDto} from "../../dtos/report/UserReportDto";
import {UserService} from "../../services/user-service.service";
import {ProfileDto} from "../../dtos/profile/ProfileDto";
import {ProfileService} from "../../services/profile-service.service";
import {ProfileCardComponent} from "../profile/profile-card/profile-card.component";

@Component({
  selector: 'app-moderator-panel',
  templateUrl: './moderator-panel.component.html',
  styleUrls: ['./moderator-panel.component.css'],
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    StatusTranslatePipe,
    ProfileCardComponent
  ]
})
export class ModeratorPanelComponent implements OnInit, OnDestroy {
  filter: ReportFilterDto = {};
  reports: UserReportDto[] = [];
  pagination: any;
  pageSize = 10;
  pageNumber = 1;
  totalPages: number = 0;
  selectedReport: UserReportDto | null = null;
  newStatus: ReportStatus = ReportStatus.Reviewed;
  moderatorComment: string = '';
  banUntil: string = '';
  selectedProfile: ProfileDto | null = null;

  readonly statuses = ReportStatus;

  constructor(private reportService: UserService, private profileService: ProfileService) {}

  ngOnInit(): void {
    this.loadReports();
  }

  ngOnDestroy(): void {}

  loadReports(): void {
    this.reportService.getPaginatedReports(this.filter, this.pageSize, this.pageNumber)
      .subscribe(res => {
        this.reports = res.reports;
        this.pagination = res.pagination;
        this.totalPages = res.pagination.TotalPages;
      });
  }

  openModeration(report: UserReportDto): void {
    this.selectedReport = report;
    this.newStatus = ReportStatus.Reviewed;
    this.moderatorComment = '';
    this.banUntil = '';
  }

  moderate(): void {
    if (!this.selectedReport) return;
    const dto = {
      reportId: this.selectedReport.id,
      status: this.newStatus,
      moderatorComment: this.moderatorComment || null,
      banUntil: this.newStatus === ReportStatus.Blocked && this.banUntil
        ? new Date(this.banUntil).toISOString()
        : null
    };
    this.reportService.moderateReport(dto).subscribe(() => {
      this.selectedReport = null;
      this.loadReports();
    });
  }

  clearFilters(): void {
    this.filter = {};
    this.loadReports();
  }

  prevPage(): void {
    if (this.pageNumber > 1) {
      this.pageNumber--;
      this.loadReports();
    }
  }

  nextPage(): void {
    if (this.pageNumber < this.totalPages) {
      this.pageNumber++;
      this.loadReports();
    }
  }

  viewProfile(profileId: number): void {
    this.profileService.getProfileById(profileId).subscribe({
      next: (profile) => {
        this.selectedProfile = profile;
      }
    });
  }

  closeModal(): void {
    this.selectedProfile = null;
  }
}
