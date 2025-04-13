import { Component } from '@angular/core';
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import { AuthService } from '../../services/auth-service.service';
import { Router } from '@angular/router';
import {NgIf} from "@angular/common";

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
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
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

  getErrorMessage(controlName: string): string {
    const control = this.registerForm.get(controlName);

    if (control && control.errors) {
      if (control.errors['required']) {
        return 'This field is required';
      } else if (control.errors['minlength']) {
        return `Minimum length is ${control.errors['minlength'].requiredLength}`;
      } else if (control.errors['maxlength']) {
        return `Maximum length is ${control.errors['maxlength'].requiredLength}`;
      }else if (control.errors['email']) {
        return `Must contain email`;
      }
    }

    return '';
  }
}
