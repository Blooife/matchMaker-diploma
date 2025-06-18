import {Component, OnDestroy, OnInit} from "@angular/core";
import {ProfileService} from "../../services/profile-service.service";
import {UserService} from "../../services/user-service.service";
import {ProfileDto} from "../../dtos/profile/ProfileDto";
import {Subscription} from "rxjs";
import {AuthService} from "../../services/auth-service.service";
import {Router} from "@angular/router";
import {NgFor, NgIf} from "@angular/common";

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.css'],
  imports: [
    NgIf,
    NgFor
  ],
  standalone: true
})
export class SettingsComponent implements OnInit, OnDestroy {
  userId: number | undefined;
  profile: ProfileDto | null = null;
  isImagesAvailable: boolean = false;
  subscriptions: Subscription[] = [];

  constructor(private profileService: ProfileService, private userService: UserService,private authService: AuthService, private router: Router) { }

  ngOnInit(): void {
    const sub = this.profileService.profile$.subscribe(
      (profileDto) =>{
        if(profileDto){
          this.profile = profileDto;
        }
      }
    )
    this.subscriptions.push(sub);

    if (this.profile?.images && this.profile?.images?.length > 0) {
      this.isImagesAvailable = true;
    }
  }

  ngOnDestroy() {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }

  deleteUserById(){
    if(this.profile?.id){
      if (confirm('Вы уверены, что хотите удалить аккаунт?')) {
        this.userService.deleteUserById(this.profile.id).subscribe(
          () =>{
            this.authService.logOut();
            this.router.navigate(['']);
          }
        );
      }
    }
  }

  goToBlacklist() {
    this.router.navigate(['/black-list']);
  }

  hasMainImage(): boolean {
    return this.profile?.images?.some(img => img.isMainImage) ?? false;
  }

  redirectToChangePassword(): void {
    this.router.navigate(['/change-password']);
  }
}
