import {Component, OnDestroy, OnInit} from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import {LanguageDto} from "../../../dtos/language/LanguageDto";
import {ProfileService} from "../../../services/profile-service.service";
import {NgForOf} from "@angular/common";
import {AddLanguageToProfileDto} from "../../../dtos/language/AddLanguageToProfileDto";
import {Subscription} from "rxjs";

@Component({
  selector: 'app-update-languages',
  templateUrl: './update-languages.component.html',
  styleUrls: ['./update-languages.component.css'],
  imports: [
    NgForOf
  ],
  standalone: true
})
export class UpdateLanguagesComponent implements OnInit, OnDestroy {
  allLanguages: LanguageDto[] = [];
  profileLanguages: LanguageDto[] = [];
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
          this.profileLanguages = profileDto.languages;
        }
      }
    )
    this.subscriptions.push(routeSub, profileSub);
    this.loadLanguages();
  }

  ngOnDestroy() {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }

  loadLanguages(): void {
    this.profileService.getAllLanguages().subscribe(
      (languages) => this.allLanguages = languages,
    );
  }

  isLanguageSelected(language: LanguageDto): boolean {
    return this.profileLanguages.some(lang => lang.id === language.id);
  }

  toggleLanguageSelection(language: LanguageDto): void {
    if (this.isLanguageSelected(language)) {
      this.removeLanguage(language);
    } else {
      this.addLanguage(language);
    }
  }

  addLanguage(language: LanguageDto): void {
    const model: AddLanguageToProfileDto = {
      profileId: this.profileId,
      languageId: language.id
    }
    this.profileService.addLanguageToProfile(model).subscribe();
  }

  removeLanguage(language: LanguageDto): void {
    this.profileService.removeLanguageFromProfile(this.profileId, language.id).subscribe();
  }
}
