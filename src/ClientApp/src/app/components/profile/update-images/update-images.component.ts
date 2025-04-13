import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ImageDto } from "../../../dtos/image/ImageDto";
import { ProfileService } from "../../../services/profile-service.service";
import { AddImageDto } from "../../../dtos/image/AddImageDto";
import { NgForOf, NgIf, NgOptimizedImage } from "@angular/common";
import { Subscription } from "rxjs";
import { ChangeMainImageDto } from "../../../dtos/image/ChangeMainImageDto";
import { MyImageCropperComponent } from "./image-cropper/image-cropper.component";

@Component({
  selector: 'app-update-images',
  templateUrl: './update-images.component.html',
  styleUrls: ['./update-images.component.css'],
  imports: [
    NgIf,
    NgOptimizedImage,
    NgForOf,
    MyImageCropperComponent
  ],
  standalone: true
})
export class UpdateImagesComponent implements OnInit, OnDestroy {
  images: ImageDto[] = [];
  selectedFile: File | null = null;
  selectedImage: ImageDto | null = null;
  profileId: string = '';
  imageChangedEvent: any = '';
  croppingImage: boolean = false;
  fileName: string = ''; // Добавьте это
  private subscriptions: Subscription[] = [];

  constructor(private profileService: ProfileService, private route: ActivatedRoute) {}

  ngOnInit(): void {
    let routeSub = this.route.paramMap.subscribe(params => {
      this.profileId = params.get('profileId')!;
    });
    let profileSub = this.profileService.profile$.subscribe(
      (profileDto) => {
        if (profileDto) {
          this.images = profileDto.images.sort((a, b) => {
            if (a.isMainImage === b.isMainImage) return 0;
            return a.isMainImage ? -1 : 1;
          });
        }
      }
    );

    this.subscriptions.push(profileSub);
    this.subscriptions.push(routeSub);
  }

  ngOnDestroy() {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }

  onFileSelected(event: any): void {
    this.fileName = event.target.files[0]?.name || ''; // Сохраните имя файла
    this.imageChangedEvent = event;
    this.croppingImage = true;
  }

  onImageCropped(file: File): void {
    this.selectedFile = file;
  }

  onCropCancelled(): void {
    this.croppingImage = false;
  }

  uploadCroppedImage(): void {
    if (this.selectedFile) {
      const model: AddImageDto = {
        profileId: this.profileId,
        file: this.selectedFile
      };

      this.profileService.addImage(model).subscribe(() => {
        this.selectedFile = null;
      });
    } else {
      alert('Please select a file first');
    }
  }

  confirmCrop(){
    if (this.selectedFile) {
      this.uploadCroppedImage();
    }
    this.croppingImage = false;
  }

  cancelCrop(){
    this.croppingImage = false;
  }

  openImage(image: ImageDto): void {
    this.selectedImage = image;
  }

  closeModal(): void {
    this.selectedImage = null;
  }

  confirmDelete(imageId: number): void {
    if (confirm('Are you sure you want to delete this image?')) {
      this.profileService.removeImage(this.profileId, imageId).subscribe(() => {
        this.closeModal();
      });
    }
  }

  makeMain(imageId: number): void {
    if (confirm('Are you sure you want to make this image main?')) {
      const model: ChangeMainImageDto = {
        profileId: this.profileId,
        imageId: imageId
      };
      this.profileService.changeMainImage(model).subscribe(() => {
        this.closeModal();
      });
    }
  }
}
