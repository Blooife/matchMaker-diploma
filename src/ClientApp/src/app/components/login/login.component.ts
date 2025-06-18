import { Component } from '@angular/core';
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import { AuthService } from '../../services/auth-service.service';
import { Router } from '@angular/router';
import {NgIf} from "@angular/common";
import {environment} from "../../constants/environment";
import {getErrorMessage} from "../profile/validators";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  imports: [
    ReactiveFormsModule,
    NgIf
  ],
  standalone: true
})
export class LoginComponent {
  loginForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email, Validators.maxLength(100)]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  async login() {
    if (this.loginForm.valid) {
      this.authService.login(this.loginForm.value).subscribe({
        next: (isLoggedIn) => {
          if (isLoggedIn) {
            this.router.navigate(['']);
          }
        },
      });
    }
  }

  loginWithGoogle() {
    const url = `https://accounts.google.com/o/oauth2/v2/auth` +
      `?client_id=${environment.googleClientId}` +
      `&redirect_uri=${environment.redirectUri}` +
      `&response_type=code` +
      `&scope=email profile`;

    window.location.href = url;
  }

  protected readonly getErrorMessage = getErrorMessage;
}
