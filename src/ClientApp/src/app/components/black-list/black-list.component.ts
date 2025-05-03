import { Component, OnDestroy, OnInit } from "@angular/core";
import { ProfileDto } from "../../dtos/profile/ProfileDto";
import { Subscription } from "rxjs";
import { ProfileService } from "../../services/profile-service.service";
import { AuthService } from "../../services/auth-service.service";
import { Router } from "@angular/router";
import { MatchService } from "../../services/match-service.service";
import { NgFor, NgIf } from "@angular/common";
import {BlackListDto} from "../../dtos/black-list/BlackListDto";
import {ProfileCardComponent} from "../profile/profile-card/profile-card.component";

@Component({
  selector: 'app-black-list',
  templateUrl: './black-list.component.html',
  styleUrls: ['./black-list.component.css'],
  imports: [
    NgIf,
    NgFor,
    ProfileCardComponent
  ],
  standalone: true
})
export class BlackListComponent implements OnInit, OnDestroy {
  profile: ProfileDto | null = null;
  blackList: BlackListDto[] = [];
  subscriptions: Subscription[] = [];
  selectedProfile: ProfileDto | null = null;

  constructor(
    private profileService: ProfileService,
    private matchService: MatchService,
    private authService: AuthService,
    private router: Router,
  ) {}

  ngOnInit(): void {
    const sub = this.profileService.profile$.subscribe(
      (profileDto) => {
        if (profileDto) {
          this.profile = profileDto;
        }
      }
    );
    this.subscriptions.push(sub);

    this.loadBlackList();
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach((sub) => sub.unsubscribe());
  }

  loadBlackList(): void {
    this.matchService.getBlackList().subscribe((blackList) => {
      this.blackList = blackList;
    });
  }

  removeFromBlackList(blockedProfileId: number): void {
    const model = {
      blockerProfileId: this.profile?.id,
      blockedProfileId: blockedProfileId,
    };

    this.matchService.removeFromBlackList(model).subscribe(() => {
      this.loadBlackList();
    });
  }

  viewProfile(profileId: number): void {
    this.profileService.getProfileById(profileId).subscribe((profile) => {
      this.selectedProfile = profile;
    })
  }

  closeModal(): void {
    this.selectedProfile = null;
  }
}
