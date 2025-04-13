import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { AuthService } from "../services/auth-service.service";
import { ProfileService } from "../services/profile-service.service";
import { ProfileDto } from "../dtos/profile/ProfileDto";
import { firstValueFrom, of } from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class ProfileCreatedGuard implements CanActivate {
  profile: ProfileDto | null = null;

  constructor(private authService: AuthService, private profileService: ProfileService, private router: Router) { }

  private async getProfile(userId: string): Promise<boolean> {
    const profile = await firstValueFrom(this.profileService.getProfileByUserId(userId));
    if (profile) {
      return true;
    } else {
      await this.router.navigate(['/create-profile', this.authService.getCurrentUserId()]);
      return true;
    }
  }

  async canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<boolean> {
    const userId = this.authService.getCurrentUserId();
    if (userId) {
      return await this.getProfile(userId);
    } else {
      const isRefreshed = await firstValueFrom(this.authService.refreshToken());
      if (isRefreshed) {
        const newUserId = this.authService.getCurrentUserId();
        if (newUserId) {
          return await this.getProfile(newUserId);
        }
      }
      this.router.navigate(['']);
      return false;
    }
  }
}
