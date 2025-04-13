import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import {AuthService} from "../services/auth-service.service";
import {firstValueFrom} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class AdminGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {

  }

  async canActivate(){
    const userId = this.authService.getCurrentUserId();
    if (userId) {
      const userRoles = this.authService.getCurrentUserRoles()
      if (userRoles?.includes("Admin") || userRoles?.includes("Moderator")) {
        return true;
      }

    }else{
      const isRefreshed = await firstValueFrom(this.authService.refreshToken());
      if (isRefreshed) {
        const userRoles = this.authService.getCurrentUserRoles()
        if (userRoles?.includes("Admin") || userRoles?.includes("Moderator")) {
          return true;
        }
      }
    }
    this.router.navigate(['']);
    return false;
  }
}
