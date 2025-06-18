import { Component, OnDestroy, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";
import {FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators} from "@angular/forms";
import {ReportStatus, StatusTranslatePipe} from "../../constants/report-status";
import {ReportFilterDto} from "../../dtos/report/ReportFilterDto";
import {UserReportDto} from "../../dtos/report/UserReportDto";
import {UserService} from "../../services/user-service.service";
import {ProfileDto} from "../../dtos/profile/ProfileDto";
import {ProfileService} from "../../services/profile-service.service";
import {ProfileCardComponent} from "../profile/profile-card/profile-card.component";
import {futureDateValidator, getErrorMessage} from "../profile/validators";

@Component({
  selector: 'app-moderator-panel',
  templateUrl: './moderator-panel.component.html',
  styleUrls: ['./moderator-panel.component.css'],
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    StatusTranslatePipe,
    ReactiveFormsModule,
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
  selectedProfile: ProfileDto | null = null;

  readonly statuses = ReportStatus;
  moderationForm!: FormGroup;

  constructor(private reportService: UserService, private profileService: ProfileService, private fb: FormBuilder) {}

  ngOnInit(): void {
    this.loadReports();
    this.initForm()
  }

  ngOnDestroy(): void {}

  initForm(): void {
    this.moderationForm = this.fb.group({
      status: [ReportStatus.Reviewed, Validators.required],
      moderatorComment: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(1000)]],
      banUntil: [null, futureDateValidator()]
    });
  }

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
  }

  moderate(): void {
    if (!this.selectedReport|| this.moderationForm.invalid) return;
    const dto = {
      reportId: this.selectedReport.id,
      status: this.moderationForm.value.status,
      moderatorComment: this.moderationForm.value.moderatorComment || null,
      banUntil: this.moderationForm.value.status === ReportStatus.Blocked && this.moderationForm.value.banUntil
        ? new Date(this.moderationForm.value.banUntil).toISOString()
        : null
    };
    this.reportService.moderateReport(dto).subscribe(() => {
      this.selectedReport = null;
      this.moderationForm.reset({
        status: ReportStatus.Reviewed,
        moderatorComment: '',
        banUntil: null
      });
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

  protected readonly getErrorMessage = getErrorMessage;
}
