import {Component, OnDestroy, OnInit} from '@angular/core';
import {ProfileService} from "../../services/profile-service.service";
import {ProfileDto} from "../../dtos/profile/ProfileDto";
import {DatePipe, NgForOf, NgIf} from "@angular/common";
import {AuthService} from "../../services/auth-service.service";
import {RouterLink} from "@angular/router";
import {Gender} from "../../constants/gender";
import {UpdateImagesComponent} from "./update-images/update-images.component";
import {ProfileCardComponent} from "./profile-card/profile-card.component";
import {Subscription} from "rxjs";

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css'],
  imports: [
    NgIf,
    RouterLink,
    DatePipe,
    NgForOf,
    UpdateImagesComponent,
    ProfileCardComponent
  ],
  standalone: true
})
export class ProfileComponent implements OnInit, OnDestroy {
  profile: ProfileDto | null = null;
  userId: string | null = '';
  loading: boolean = true;
  protected readonly Gender = Gender;
  private subscriptions: Subscription[] = [];

  constructor(private profileService: ProfileService, private authService: AuthService) { }

  ngOnInit(): void {
    const sub = this.profileService.profile$.subscribe(
      (profileDto) =>{
        this.profile = profileDto;
        this.loading = false;
      }
    )
    this.subscriptions.push(sub);
  }

  ngOnDestroy() {
    this.subscriptions.forEach(sub => sub.unsubscribe())
  }
}
