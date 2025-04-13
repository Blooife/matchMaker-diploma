import {Component, OnDestroy, OnInit} from '@angular/core';
import { ProfileEducationDto } from "../../../dtos/education/ProfileEducationDto";
import { EducationDto } from "../../../dtos/education/EducationDto";
import { ProfileDto } from "../../../dtos/profile/ProfileDto";
import { ProfileService } from "../../../services/profile-service.service";
import { ActivatedRoute } from "@angular/router";
import { UpdateProfileEducationDto } from "../../../dtos/education/UpdateProfileEducationDto";
import { AddEducationToProfileDto } from "../../../dtos/education/AddEducationToProfileDto";
import { FormsModule } from "@angular/forms";
import { NgForOf, NgIf } from "@angular/common";
import {Subscription} from "rxjs";

@Component({
  selector: 'app-update-education',
  templateUrl: './update-education.component.html',
  styleUrls: ['./update-education.component.css'],
  imports: [
    FormsModule,
    NgForOf,
    NgIf
  ],
  standalone: true
})
export class UpdateEducationComponent implements OnInit, OnDestroy {
  educations: EducationDto[] = [];
  profileEducations: ProfileEducationDto[] = [];
  educationDescriptions: { [key: number]: string } = {};
  profileId: string = '';
  profile: ProfileDto | null = null;
  private subscriptions: Subscription[] = [];

  constructor(private profileService: ProfileService, private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.profileId = params.get('profileId')!;
    });
    let routeSub = this.route.paramMap.subscribe(params => {
      this.profileId = params.get('profileId')!;
    });
    let profileSub = this.profileService.profile$.subscribe(
      (profileDto) =>{
        this.profile = profileDto;
        this.profileEducations = profileDto!.education;
        this.profileEducations.forEach(e => {
          this.educationDescriptions[e.educationId] = e.description;
        });
      }
    )
    this.subscriptions.push(routeSub);
    this.subscriptions.push(profileSub);

    this.loadEducations();
  }

  ngOnDestroy() {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }

  loadEducations(): void {
    this.profileService.getAllEducations().subscribe((educations: EducationDto[]) => {
      this.educations = educations;
    });
  }

  isEducationSelected(educationId: number): boolean {
    return this.profileEducations.some(e => e.educationId === educationId);
  }

  getEducationDescription(educationId: number): string {
    const education = this.profileEducations.find(e => e.educationId === educationId);
    return education ? education.description : '';
  }

  onEducationToggle(education: EducationDto): void {
    const isSelected = this.isEducationSelected(education.id);
    if (isSelected) {
      this.removeEducation(education.id);
    } else {
      const newEducation: ProfileEducationDto = {
        profileId: this.profileId,
        educationId: education.id,
        educationName: education.name,
        description: this.getEducationDescription(education.id)
      };
      this.addEducation(newEducation);
    }
  }

  onDescriptionChange(educationId: number, description: string): void {
    this.educationDescriptions[educationId] = description;
    const index = this.profileEducations.findIndex(e => e.educationId === educationId);
    if (index > -1) {
      this.profileEducations[index].description = description;
    }
  }

  addEducation(education: ProfileEducationDto): void {
    const model: AddEducationToProfileDto = {
      profileId: this.profileId,
      educationId: education.educationId,
      description: education.description
    };
    this.profileService.addEducationToProfile(model).subscribe();
  }

  removeEducation(educationId: number): void {
    this.profileService.removeEducationFromProfile(this.profileId, educationId).subscribe();
  }

  save(): void {
    this.profileEducations.forEach(education => {
      const model: UpdateProfileEducationDto = {
        profileId: this.profileId,
        educationId: education.educationId,
        description: education.description
      };
      this.profileService.updateProfileEducation(model).subscribe(() => {

      });
    });
    alert('Profile education updated successfully');
  }
}
