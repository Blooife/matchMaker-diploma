import { Component, EventEmitter, Output, Input } from '@angular/core';
import { ImageCroppedEvent, ImageCropperComponent } from 'ngx-image-cropper';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-image-cropper',
  templateUrl: './image-cropper.component.html',
  styleUrls: ['./image-cropper.component.css'],
  imports: [CommonModule, ImageCropperComponent],
  standalone: true
})
export class MyImageCropperComponent {
  @Input() imageChangedEvent: any;
  @Input() fileName: string = '';
  @Output() imageCroppedEvent = new EventEmitter<File>();
  @Output() cropCancelled = new EventEmitter<void>();
  aspectRatio: number = 4 / 3;

  imageCropped(event: ImageCroppedEvent) {
    const blob = event.blob;
      if(blob){
      const file = new File([blob], this.fileName, { type: 'image/png' });
      this.imageCroppedEvent.emit(file);
    }
  }

  private base64ToBlob(base64: string): Blob {
    const byteString = window.atob(base64.split(',')[1]);
    const arrayBuffer = new ArrayBuffer(byteString.length);
    const int8Array = new Uint8Array(arrayBuffer);
    for (let i = 0; i < byteString.length; i++) {
      int8Array[i] = byteString.charCodeAt(i);
    }
    return new Blob([int8Array],{ type: 'image/png' });
  }
}
