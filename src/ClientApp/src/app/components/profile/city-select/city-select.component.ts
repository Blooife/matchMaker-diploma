import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';
import { CityDto } from "../../../dtos/city/CityDto";
import { ProfileService } from "../../../services/profile-service.service";
import { NgForOf, NgIf } from "@angular/common";
import {FormsModule} from "@angular/forms";

@Component({
  selector: 'app-city-select',
  templateUrl: './city-select.component.html',
  styleUrls: ['./city-select.component.css'],
  imports: [
    NgIf,
    NgForOf,
    FormsModule
  ],
  standalone: true
})
export class CitySelectComponent implements OnChanges {
  @Input() countryId: number | null = null;
  @Input() selectedCityId: number | null = null;
  @Output() citySelected = new EventEmitter<number>();

  cities: CityDto[] = [];

  constructor(private profileService: ProfileService) {}

  ngOnChanges(changes: SimpleChanges) {
    if (changes['countryId'] && this.countryId !== null) {
      this.loadCities();
    }
  }

  async loadCities() {
    if(this.countryId){
      this.profileService.getCitiesByCountryId(this.countryId).subscribe(
        {next:(result) =>{
            this.cities =result;
          }}
      );
    }else {
      this.cities = [];
    }
  }

  onCityChange(event: Event) {
    const selectElement = event.target as HTMLSelectElement;
    const value = selectElement.value;
    const id = this.extractCityId(value);

    if(id === null){
      this.selectedCityId = null;
      this.citySelected.emit(undefined);
    }else{
      this.selectedCityId = Number(id);
      this.citySelected.emit(Number(id));
    }

  }

  private extractCityId(value: string): string | null {
    const parts = value.split(':');
    return parts.length > 1 ? parts[1].trim() : null;
  }
}
