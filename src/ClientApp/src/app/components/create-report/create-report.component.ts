import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {NgIf, NgFor } from "@angular/common";
import { FormsModule } from "@angular/forms";
import {UserService} from "../../services/user-service.service";
import {ReportTypeDto} from "../../dtos/report/ReportTypeDto";
import {CreateUserReportDto} from "../../dtos/report/CreateUserReportDto";

@Component({
  selector: 'app-create-report',
  templateUrl: './create-report.component.html',
  styleUrls: ['./create-report.component.css'],
  standalone: true,
  imports: [NgIf, NgFor, FormsModule]
})
export class CreateReportComponent implements OnInit {
  @Input() profileId!: number;
  @Input() visible: boolean = false;
  reportTypes: ReportTypeDto[] = [];
  selectedTypeId: number | null = null;
  comment: string = '';
  isSubmitting: boolean = false;
  @Output() clickOutside = new EventEmitter<unknown>();

  constructor(private userService: UserService) {}

  ngOnInit(): void {
    this.userService.getReportTypes().subscribe(types => {
      this.reportTypes = types;
    });
  }

  submitReport(): void {
    if (!this.selectedTypeId || !this.comment.trim()) return;

    const model: CreateUserReportDto = {
      reportedUserId: this.profileId,
      reportTypeId: this.selectedTypeId,
      reason: this.comment
    };

    this.isSubmitting = true;
    this.userService.createReport(model).subscribe({
      next: () => {
        this.visible = false;
        this.resetForm();
      },
      error: () => {
        this.isSubmitting = false;

      }
    });
  }

  close(): void {
    this.visible = false;
    this.resetForm();
  }

  private resetForm(): void {
    this.selectedTypeId = null;
    this.comment = '';
    this.isSubmitting = false;
  }
}
