import {Component, EventEmitter, Input, OnDestroy, OnInit, Output} from '@angular/core';
import {NgIf, NgFor } from "@angular/common";
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {UserService} from "../../services/user-service.service";
import {ReportTypeDto} from "../../dtos/report/ReportTypeDto";
import {CreateUserReportDto} from "../../dtos/report/CreateUserReportDto";
import {futureDateValidator, getErrorMessage} from "../profile/validators";

@Component({
  selector: 'app-create-report',
  templateUrl: './create-report.component.html',
  styleUrls: ['./create-report.component.css'],
  standalone: true,
  imports: [NgIf, NgFor, ReactiveFormsModule,]
})
export class CreateReportComponent implements OnInit, OnDestroy {
  @Input() profileId!: number;
  @Input() visible: boolean = false;
  @Output() clickOutside = new EventEmitter<unknown>();

  reportTypes: ReportTypeDto[] = [];
  isSubmitting: boolean = false;

  form!: FormGroup;

  constructor(private userService: UserService, private fb: FormBuilder) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      reportTypeId: [null, Validators.required],
      comment: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(1000)]],
    });

    this.userService.getReportTypes().subscribe(types => {
      this.reportTypes = types;
    });
  }

  ngOnDestroy() {
    this.close();
  }

  submitReport(): void {
    if (this.form.invalid) return;

    const model: CreateUserReportDto = {
      reportedUserId: this.profileId,
      reportTypeId: this.form.value.reportTypeId,
      reason: this.form.value.comment
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
    this.clickOutside.emit();
  }

  private resetForm(): void {
    this.form.reset();
    this.isSubmitting = false;
  }

  protected readonly getErrorMessage = getErrorMessage;
}
