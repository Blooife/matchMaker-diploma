import {Component, OnDestroy, OnInit} from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import {ProfileService} from "../../../services/profile-service.service";
import {NgForOf} from "@angular/common";
import {AddInterestToProfileDto} from "../../../dtos/interest/AddInterestToProfileDto";
import {InterestDto} from "../../../dtos/interest/InterestDto";
import {Subscription} from "rxjs";

@Component({
  selector: 'app-update-interests',
  templateUrl: './update-interests.component.html',
  styleUrls: ['./update-interests.component.css'],
  imports: [
    NgForOf
  ],
  standalone: true
})
export class UpdateInterestsComponent implements OnInit, OnDestroy {
  allInterests: InterestDto[] = [];
  profileInterests: InterestDto[] = [];
  profileId: string = '';
  private subscriptions: Subscription[] = [];

  constructor(private profileService: ProfileService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    let routeSub = this.route.paramMap.subscribe(params => {
      this.profileId = params.get('profileId')!;
    });
    let profileSub = this.profileService.profile$.subscribe(
      (profileDto) =>{
        if(profileDto){
          this.profileInterests = profileDto.interests;
        }
      }
    )
    this.subscriptions.push(routeSub, profileSub);
    this.loadInterests();
  }

  ngOnDestroy() {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }

  loadInterests(): void {
    this.profileService.getAllInterests().subscribe(
      (interests) => this.allInterests = interests,
    );
  }

  isInterestSelected(interest: InterestDto): boolean {
    return this.profileInterests.some(lang => lang.id === interest.id);
  }

  toggleInterestSelection(interest: InterestDto): void {
    if (this.isInterestSelected(interest)) {
      this.removeLanguage(interest.id);
    } else {
      this.addLanguage(interest);
    }
  }

  addLanguage(interest: InterestDto): void {
    const model: AddInterestToProfileDto = {
      profileId: this.profileId,
      interestId: interest.id
    }
    this.profileService.addInterestToProfile(model).subscribe();
  }

  removeLanguage(interestId: number): void {
    this.profileService.removeInterestFromProfile(this.profileId, interestId).subscribe();
  }
}
