import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import {formatDate, KeyValuePipe, NgForOf} from '@angular/common';
import { Gender } from '../../../constants/gender';
import { ProfileService } from '../../../services/profile-service.service';
import { ActivatedRoute, Router } from '@angular/router';
import { CountrySelectComponent } from '../country-select/country-select.component';
import { CitySelectComponent } from '../city-select/city-select.component';
import { GoalSelectComponent } from '../goal-select/goal-select.component';
import {UpdateProfileDto} from "../../../dtos/profile/UpdateProfileDto";
import {UpdateInterestsComponent} from "../update-interests/update-interests.component";
import {UpdateLanguagesComponent} from "../update-languages/update-languages.component";
import {UpdateEducationComponent} from "../update-education/update-education.component";
import {UpdateImagesComponent} from "../update-images/update-images.component";
import {minimumAge, rangeValidator, minValue, ageFromLessThanOrEqualAgeTo, getErrorMessage} from '../validators';

@Component({
  selector: 'app-update-profile',
  templateUrl: './update-profile.component.html',
  styleUrls: ['../create-profile/create-profile.component.css'],
  imports: [
    NgForOf,
    ReactiveFormsModule,
    KeyValuePipe,
    CountrySelectComponent,
    CitySelectComponent,
    GoalSelectComponent,
    UpdateInterestsComponent,
    UpdateLanguagesComponent,
    UpdateEducationComponent,
    UpdateImagesComponent
  ],
  standalone: true
})
export class UpdateProfileComponent implements OnInit {
  profileForm: FormGroup;
  genders = Object.keys(Gender)
    .filter(key => isNaN(Number(key)))
    .map(key => ({ key, value: Gender[key as keyof typeof Gender] }));
  profileId: string = '';
  selectedCountryId: number | null = null;
  selectedCityId: number | null = null;
  selectedGoalId: number | null = null;

  constructor(private fb: FormBuilder, private route: ActivatedRoute, private router: Router, private profileService: ProfileService) {
    this.profileForm = this.fb.group({
      id: ['', Validators.required],
      name: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(50)]],
      lastName: ['', [Validators.minLength(2), Validators.maxLength(50)]],
      birthDate: ['', [Validators.required, minimumAge(16)]],
      gender: ['', Validators.required],
      bio: [null, [Validators.minLength(10), Validators.maxLength(500)]],
      height: [null, [rangeValidator(100, 220)]],
      showAge: [true, Validators.required],
      ageFrom: ['', [Validators.required, minValue(0)]],
      ageTo: ['', [Validators.required, minValue(0)]],
      maxDistance: ['', [Validators.required, minValue(0)]],
      preferredGender: ['', Validators.required],
      goalId: [null],
      cityId: [null, Validators.required],
      userId: ['', Validators.required]
    }, {
      validators: ageFromLessThanOrEqualAgeTo('ageFrom', 'ageTo')
    });
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.profileId = params.get('profileId')!;
      this.loadProfile();
    });
  }

  async loadProfile() {
    this.profileService.profile$.subscribe(
      (profile) =>{
        if(profile){
          this.profileForm.patchValue({
            id: profile.id,
            name: profile.name,
            lastName: profile.lastName,
            birthDate: formatDate(profile.birthDate, 'yyyy-MM-dd', 'en-US'),
            gender: profile.gender,
            bio: profile.bio,
            height: profile.height,
            showAge: profile.showAge,
            ageFrom: profile.ageFrom,
            ageTo: profile.ageTo,
            maxDistance: profile.maxDistance,
            preferredGender: profile.preferredGender,
            goalId: profile.goal?.id,
            cityId: profile.city.id,
            userId: profile.userId
          });
          this.selectedCountryId = profile.country.id ?? null;
          this.selectedCityId = profile.city.id ?? null;
          this.selectedGoalId = profile.goal?.id ?? null;
        }
      }
      );
  }

  onSubmit() {
    if (this.profileForm.valid) {
      const updateProfileDto: UpdateProfileDto = {
        ...this.profileForm.value,
        birthDate: new Date(this.profileForm.value.birthDate).toISOString(),
        gender: Number(this.profileForm.value.gender),
        preferredGender: Number(this.profileForm.value.preferredGender),
        id: this.profileId
      };
      this.profileService.updateProfile(updateProfileDto).subscribe({
        next: (response) => {
          this.router.navigate(['/profile']);
        }
      });
    }else{
      this.profileForm.markAllAsTouched();
    }
  }

  onCountrySelected(countryId: number) {
    this.selectedCountryId = countryId;
    this.profileForm.patchValue({ cityId: "" });
  }

  onCitySelected(cityId: number) {
    this.profileForm.patchValue({ cityId });
  }

  onGoalSelected(goalId: number) {
    this.profileForm.patchValue({ goalId });
  }

  protected readonly getErrorMessage = getErrorMessage;
}
