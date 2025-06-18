import { Component, OnDestroy, OnInit } from '@angular/core';
import { AuthService } from '../../../services/auth-service.service';
import { Router } from '@angular/router';
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import { Subscription } from 'rxjs';
import {ChangePasswordRequestDto} from "../../../dtos/auth/changePasswordRequestDto";
import {NgIf} from "@angular/common";
import {getErrorMessage, passwordMatchValidator} from "../../profile/validators";

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.css'],
  imports: [
    ReactiveFormsModule,
    NgIf
  ],
  standalone: true
})
export class ChangePasswordComponent implements OnInit, OnDestroy {
  changePasswordForm: FormGroup;
  private subscription: Subscription = new Subscription();

  constructor(
    private authService: AuthService,
    private fb: FormBuilder,
    private router: Router
  ) {
    this.changePasswordForm = this.fb.group({
      currentPassword: ['', [Validators.required, Validators.minLength(6)]],
      newPassword: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', [Validators.required, Validators.minLength(6)]],
    }, { validators: passwordMatchValidator('newPassword', 'confirmPassword') });
  }

  ngOnInit(): void {}

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }



  onSubmit(): void {
    if (this.changePasswordForm.invalid) {
      return;
    }

    const model: ChangePasswordRequestDto = {
      currentPassword: this.changePasswordForm.value.currentPassword,
      newPassword: this.changePasswordForm.value.newPassword
    };

    this.subscription.add(
      this.authService.changePassword(model).subscribe({
        next: () => {
          this.changePasswordForm.reset();
          setTimeout(() => {
            this.router.navigate(['/profile']);
          }, 2000);
        }
      })
    );
  }

  protected readonly getErrorMessage = getErrorMessage;
}
