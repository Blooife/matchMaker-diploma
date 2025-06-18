import { Component } from '@angular/core';
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import { AuthService } from '../../services/auth-service.service';
import { Router } from '@angular/router';
import {NgIf} from "@angular/common";
import {getErrorMessage, passwordMatchValidator} from "../profile/validators";

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
  imports: [
    ReactiveFormsModule,
    NgIf
  ],
  standalone: true
})
export class RegisterComponent {
  registerForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    this.registerForm = this.fb.group({
      email: ['', [Validators.required, Validators.email, Validators.maxLength(100)]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', [Validators.required, Validators.minLength(6)]],
    }, { validators: passwordMatchValidator('password', 'confirmPassword') });
  }

  async register() {
    this.authService.register(this.registerForm.value).subscribe(
      {
        next: (result) => {
          if (result) {
            this.router.navigate(['/login']);
          }
        },
      }
    )
  }

  protected readonly getErrorMessage = getErrorMessage;
}
