import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import {AuthService} from "../services/auth-service.service";

@Injectable({
  providedIn: 'root'
})
export class LoggedInGuard implements CanActivate {

  constructor(private authService: AuthService, private router: Router) {

  }

  canActivate(): boolean {
    if (this.authService.isLogged()) {
      const userRoles = this.authService.getCurrentUserRoles()
      if (userRoles?.includes("Admin") || userRoles?.includes("Moderator")) {
        this.router.navigate(['/admin-panel']);
        return false;
      } else {
        this.router.navigate(['/profile']);
        return false;
      }
    } else {
      return true;
    }
  }
}
