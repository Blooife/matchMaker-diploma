import {Component, OnDestroy, OnInit} from '@angular/core';
import { ProfileService } from "../../services/profile-service.service";
import { AddLikeDto } from "../../dtos/like/AddLikeDto";
import { MatchService } from "../../services/match-service.service";
import { CommonModule } from "@angular/common";
import { ActivatedRoute } from "@angular/router";
import { AuthService } from "../../services/auth-service.service";
import { ProfileDto } from "../../dtos/profile/ProfileDto";
import { ProfileCardComponent } from "../profile/profile-card/profile-card.component";
import { animate, state, style, transition, trigger } from "@angular/animations";
import {UpdateLocationDto} from "../../dtos/match/UpdateLocationDto";
import {Subscription} from "rxjs";

@Component({
  selector: 'app-find-match',
  templateUrl: './find-match.component.html',
  styleUrls: ['./find-match.component.css'],
  imports: [
    CommonModule,
    ProfileCardComponent,
  ],
  standalone: true,
  animations: [
    trigger('flyInOut', [
      state('center', style({ opacity: 1, transform: 'translateX(0)' })),
      transition('void => center', [
        style({ opacity: 0, transform: 'translateX(-100%)' }),
        animate('300ms ease-in')
      ]),
      transition('center => like', [
        animate('300ms ease-out', style({ opacity: 0, transform: 'translateX(100%)' }))
      ]),
      transition('center => dislike', [
        animate('300ms ease-out', style({ opacity: 0, transform: 'translateX(-100%)' }))
      ])
    ])
  ]
})
export class FindMatchComponent implements OnInit, OnDestroy {
  profileId: number | undefined;
  profile: ProfileDto | undefined;
  userId: number | undefined;
  recommendations: ProfileDto[] = [];
  currentIndex: number = 0;
  noMoreRecs: boolean = false;
  animationState: string = 'center';
  recsLoading: boolean = true;
  subscriptions: Subscription[] = [];
  compatibilityResult: any;

  constructor(private matchService: MatchService,
              private route: ActivatedRoute,
              private authService: AuthService,
              private profileService: ProfileService) { }

  ngOnInit(): void {
    const sub = this.matchService.recsLoading$.subscribe(
      (loading) =>{
        this.recsLoading = loading;
      }
    )
    const sub2 = this.profileService.profile$.subscribe(
      (profileDto) =>{
        if(profileDto){
          this.profileId = profileDto?.id;
          this.profile = profileDto;
        }
      }
    )
    this.subscriptions.push(sub, sub2);

    this.loadProfile().then(async () => {
      await this.requestLocation();
      this.loadRecommendations();
    });
  }

  ngOnDestroy() {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }

  loadRecommendations(): void {
    this.matchService.getRecs().subscribe({
      next: (recs) => {
        if (recs.length === 0) {
          this.noMoreRecs = true;
        } else {
          this.recommendations = recs;
          this.currentIndex = 0;
        }
      }
    });
  }

  addLike(targetProfileId: number, isLike: boolean): void {
    const model: AddLikeDto = {
      profileId: this.profileId!,
      targetProfileId: targetProfileId,
      isLike: isLike
    };
    this.matchService.addLike(model).subscribe({
      next: () => {
        this.currentIndex++;
        if (this.currentIndex >= this.recommendations.length) {
          this.loadRecommendations();
        } else {
          this.animationState = 'center';
        }
      }
    });
  }

  onLike(targetProfileId: number): void {
    this.animationState = 'like';
    setTimeout(() => {
      this.addLike(targetProfileId, true);
    }, 300);
  }

  onDislike(targetProfileId: number): void {
    this.animationState = 'dislike';
    setTimeout(() => {
      this.addLike(targetProfileId, false);
    }, 300);
  }

  async loadProfile() {
    this.userId = this.authService.getCurrentUserId()!;
    if (this.userId) {
      const profile = await this.profileService.getProfileByUserId(this.userId).toPromise();
      if (profile) {
        this.profileId = profile.id;
      }
    }
  }

  async requestLocation(): Promise<void> {
    return new Promise<void>((resolve, reject) => {
      if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(
          (position) => {
            const model: UpdateLocationDto = {
              profileId: this.profileId!,
              latitude: position.coords.latitude,
              longitude: position.coords.longitude
            };
            this.matchService.updateLocation(model).subscribe({
              next: () => {
                console.log('Location updated successfully');
                resolve();
              },
              error: (error) => {
                console.error('Error updating location:', error);
                resolve();
              }
            });
          },
          (error) => {
            console.warn('User denied geolocation request, sending request with null values');
            const model: UpdateLocationDto = {
              profileId: this.profileId!,
              latitude: null,
              longitude: null
            };
            this.matchService.updateLocation(model).subscribe({
              next: () => {
                resolve();
              },
              error: (error) => {
                console.error('Error updating location with null values:', error);
                resolve();
              }
            });
          }
        );
      } else {
        console.error('Geolocation is not supported by this browser.');
        const model: UpdateLocationDto = {
          profileId: this.profileId!,
          latitude: null,
          longitude: null
        };
        this.matchService.updateLocation(model).subscribe({
          next: () => {
            console.log('Location update request sent with null values');
            resolve();
          },
          error: (error) => {
            console.error('Error updating location with null values:', error);
            resolve();
          }
        });
      }
    });
  }

  loadCompatibility(partner: ProfileDto) {
    const myBirthDate = this.profile?.birthDate;
    const partnerBirthDate = partner.birthDate;

    if (!myBirthDate || !partnerBirthDate) {
      alert('Нет данных о дате рождения для совместимости.');
      return;
    }

    this.matchService.getCompatibility(myBirthDate, partnerBirthDate).subscribe({
      next: (result) => {
        this.compatibilityResult = result;
        const dialog: any = document.querySelector('.compatibility-modal');
        if (dialog) dialog.showModal();
      },
      error: () => {
        alert('Ошибка при загрузке совместимости.');
      }
    });
  }

  closeModal() {
    const dialog: any = document.querySelector('.compatibility-modal');
    if (dialog) dialog.close();
  }
}
