import {Component, Input} from '@angular/core';
import {DatePipe, NgForOf, NgIf, NgOptimizedImage} from "@angular/common";
import {RouterLink} from "@angular/router";
import {UpdateImagesComponent} from "../update-images/update-images.component";
import {ProfileDto} from "../../../dtos/profile/ProfileDto";
import { Gender } from '../../../constants/gender';

@Component({
  selector: 'app-profile-card',
  templateUrl: './profile-card.component.html',
  styleUrls: ['./profile-card.component.css'],
  imports: [
    NgIf,
    RouterLink,
    DatePipe,
    NgForOf,
    UpdateImagesComponent,
    NgOptimizedImage
  ],
  standalone: true
})
export class ProfileCardComponent {
  @Input() profile!: ProfileDto;
  currentImageIndex: number = 0;
  protected readonly Gender = Gender;

  constructor() { }

  prevImage() {
    if (this.profile && this.profile.images.length > 0) {
      this.currentImageIndex = (this.currentImageIndex - 1 + this.profile.images.length) % this.profile.images.length;
    }
  }

  nextImage() {
    if (this.profile && this.profile.images.length > 0) {
      this.currentImageIndex = (this.currentImageIndex + 1) % this.profile.images.length;
    }
  }
}
