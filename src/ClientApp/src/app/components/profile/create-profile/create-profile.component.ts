import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ProfileService } from '../../../services/profile-service.service';
import { CreateProfileDto } from '../../../dtos/profile/CreateProfileDto';
import { Gender } from '../../../constants/gender';
import { CountrySelectComponent } from '../country-select/country-select.component';
import { CitySelectComponent } from '../city-select/city-select.component';
import { GoalSelectComponent } from '../goal-select/goal-select.component';
import { MatButton } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatSliderModule } from '@angular/material/slider';
import {NgForOf} from "@angular/common";
import {ageFromLessThanOrEqualAgeTo, getErrorMessage, minimumAge, minValue, rangeValidator} from "../validators";

@Component({
  selector: 'app-create-profile',
  templateUrl: './create-profile.component.html',
  styleUrls: ['./create-profile.component.css'],
  standalone: true,
  imports: [
    MatInputModule,
    MatSelectModule,
    MatSliderModule,
    CountrySelectComponent,
    CitySelectComponent,
    GoalSelectComponent,
    MatButton,
    ReactiveFormsModule,
    NgForOf
  ]
})
export class CreateProfileComponent implements OnInit {
  profileForm: FormGroup;
  genders = Object.keys(Gender)
    .filter(key => isNaN(Number(key)))
    .map(key => ({ key, value: Gender[key as keyof typeof Gender] }));
  userId: string = '';
  selectedCountryId: number | null = null;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private profileService: ProfileService
  ) {
    this.profileForm = this.fb.group({
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
    }, {
      validators: ageFromLessThanOrEqualAgeTo('ageFrom', 'ageTo')
    });
  }

  ngOnInit(): void {
    console.log(this.genders)
    this.route.paramMap.subscribe(params => {
      this.userId = params.get('userId')!;
    });
  }

  onSubmit() {
    if (this.profileForm.valid) {
      console.log("valid")
      const createProfileDto: CreateProfileDto = {
        ...this.profileForm.value,
        birthDate: new Date(this.profileForm.value.birthDate).toISOString(),
        gender: Number(this.profileForm.value.gender),
        preferredGender: Number(this.profileForm.value.preferredGender),
        userId: this.userId
      };


      this.profileService.createProfile(createProfileDto).subscribe({
        next: () => {
          this.router.navigate(['/profile']);
        }
      });
    }else {
      console.log("not valid")
      this.profileForm.markAllAsTouched();
    }
  }

  onCountrySelected(countryId: number) {
    this.selectedCountryId = countryId;
    this.profileForm.patchValue({ cityId: '' });
  }

  onCitySelected(cityId: number) {
    this.profileForm.patchValue({ cityId });
  }

  onGoalSelected(goalId: number) {
    this.profileForm.patchValue({ goalId });
  }

  protected readonly getErrorMessage = getErrorMessage;
}
