import {Component, OnDestroy, OnInit} from '@angular/core';
import { ProfileService } from "../../services/profile-service.service";
import { MatchService } from "../../services/match-service.service";
import { CommonModule } from "@angular/common";
import {ActivatedRoute, Router} from "@angular/router";
import { ProfileDto } from "../../dtos/profile/ProfileDto";
import { ProfileCardComponent } from "../profile/profile-card/profile-card.component";
import {Subscription} from "rxjs";

@Component({
  selector: 'app-matches',
  templateUrl: './matches.component.html',
  styleUrls: ['./matches.component.css'],
  imports: [
    CommonModule,
    ProfileCardComponent,
  ],
  standalone: true,
})
export class MatchesComponent implements OnInit, OnDestroy {
  profile: ProfileDto | null = null;
  matches: ProfileDto[] = [];
  subscriptions: Subscription[] = [];
  pageSize: number = 10;
  pageNumber: number = 1;
  totalPages: number = 0;
  selectedProfile: ProfileDto | null = null;

  constructor(private matchService: MatchService,  private route: ActivatedRoute, private router: Router,  private profileService: ProfileService) { }

  ngOnInit(): void {
    const sub2 = this.profileService.profile$.subscribe(
      (profileDto) =>{
        if(profileDto){
          this.profile = profileDto;
        }
      }
    )
    this.subscriptions.push(sub2);
    this.loadMatches();
  }

  ngOnDestroy() {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }

  loadMatches(): void {
    this.matchService.getPaginatedMatches(this.pageSize, this.pageNumber, this.profile!.id).subscribe({
      next: (result) => {
        this.matches = result.matches;
        this.totalPages = result.pagination.TotalPages;
      }
    });
  }

  viewProfile(profile: ProfileDto): void {
    this.selectedProfile = profile;
  }

  prevPage(): void {
    if (this.pageNumber > 1) {
      this.pageNumber--;
      this.loadMatches();
    }
  }

  nextPage(): void {
    if (this.pageNumber < this.totalPages) {
      this.pageNumber++;
      this.loadMatches();
    }
  }

  closeModal(): void {
    this.selectedProfile = null;
  }

  startDialog(selectedProfileId: string){
    if (this.selectedProfile) {
      const currentProfileId = this.profile?.id!;
      this.router.navigate(['/chats'], {
        queryParams: { firstProfileId: currentProfileId, secondProfileId: selectedProfileId }
      });
    }
  }
}
