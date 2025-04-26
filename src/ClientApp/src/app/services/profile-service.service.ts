import {Injectable, model} from '@angular/core';
import {HttpClient, HttpHeaders, HttpErrorResponse, HttpContext, HttpContextToken} from '@angular/common/http';
import {Observable, throwError, of, BehaviorSubject} from 'rxjs';
import {catchError, retry, tap} from 'rxjs/operators';
import { GeneralResponseDto } from "../dtos/shared/generalResponseDto";
import {
  countriesEndpoints, educationEndpoints,
  goalsEndpoints, imagesEndpoints,
  interestsEndpoints,
  languagesEndpoints,
  profilesEndpoints
} from "../constants/api-endpoints";
import { UpdateProfileDto } from "../dtos/profile/UpdateProfileDto";
import { CreateProfileDto } from "../dtos/profile/CreateProfileDto";
import { ProfileDto } from "../dtos/profile/ProfileDto";
import { CountryDto } from "../dtos/country/CountryDto";
import { CityDto } from "../dtos/city/CityDto";
import { GoalDto } from "../dtos/goal/GoalDto";
import {LanguageDto} from "../dtos/language/LanguageDto";
import {AddLanguageToProfileDto} from "../dtos/language/AddLanguageToProfileDto";
import {InterestDto} from "../dtos/interest/InterestDto";
import {AddInterestToProfileDto} from "../dtos/interest/AddInterestToProfileDto";
import {EducationDto} from "../dtos/education/EducationDto";
import {AddEducationToProfileDto} from "../dtos/education/AddEducationToProfileDto";
import {UpdateProfileEducationDto} from "../dtos/education/UpdateProfileEducationDto";
import {AddImageDto} from "../dtos/image/AddImageDto";
import {ImageDto} from "../dtos/image/ImageDto";
import {ProfileEducationDto} from "../dtos/education/ProfileEducationDto";
import {_IGNORED_STATUSES} from "../constants/http-context";
import {ChangeMainImageDto} from "../dtos/image/ChangeMainImageDto";

@Injectable({
  providedIn: 'root'
})
export class ProfileService {
  private httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  private profileSubject = new BehaviorSubject<ProfileDto|null>(null);
  profile$ = this.profileSubject.asObservable();


  constructor(private httpClient: HttpClient) { }

  getProfileById(profileId: number): Observable<ProfileDto> {
    return this.httpClient.get<ProfileDto>(`${profilesEndpoints.profiles}/${profileId}`, this.httpOptions)
      .pipe();
  }

  getProfileByUserId(userId: number): Observable<ProfileDto | null> {
    return this.httpClient.get<ProfileDto>(profilesEndpoints.byUserId(userId), {
      headers: this.httpOptions.headers,
      context: new HttpContext().set(_IGNORED_STATUSES, true),
    })
      .pipe(
        tap(result => {
          this.profileSubject.next(result);
        }),
        catchError(error => {
          if (error.status === 404) {
            this.profileSubject.next(null);
            return of(null);
          } else {
            return throwError(() => error);
          }
        })
      );
  }

  updateProfile(model: UpdateProfileDto): Observable<ProfileDto> {
    return this.httpClient.put<ProfileDto>(`${profilesEndpoints.profiles}/${model.id}`, model, this.httpOptions)
      .pipe(
        tap(result => {
          this.profileSubject.next(result);
        }),
      );
  }

  createProfile(model: CreateProfileDto): Observable<ProfileDto> {
    return this.httpClient.post<ProfileDto>(`${profilesEndpoints.profiles}`, model, this.httpOptions)
      .pipe(
        tap(result => {
          this.profileSubject.next(result);
        }),
      );
  }

  getAllCountries(): Observable<CountryDto[]> {
    return this.httpClient.get<CountryDto[]>(`${countriesEndpoints.countries}`, this.httpOptions)
      .pipe(
      );
  }

  getAllLanguages(): Observable<LanguageDto[]> {
    return this.httpClient.get<LanguageDto[]>(`${languagesEndpoints.languages}`, this.httpOptions)
      .pipe(
      );
  }

  addLanguageToProfile(model: AddLanguageToProfileDto): Observable<LanguageDto[]> {
        return this.httpClient.post<LanguageDto[]>(`${languagesEndpoints.profilesLanguages}`, model, this.httpOptions)
      .pipe(
        tap(result => {
          const currentProfile = this.profileSubject.value;
          if (currentProfile) {
            const updatedProfile: ProfileDto = {
              ...currentProfile,
              languages: result
            };
            this.profileSubject.next(updatedProfile);
          }
        }),
      );
  }

  removeLanguageFromProfile(profileId: number, languageId: number): Observable<LanguageDto[]> {
    const options = {
      headers: this.httpOptions.headers,
      body: {
        profileId: profileId,
        languageId: languageId
      },
    };

    return this.httpClient.delete<LanguageDto[]>(`${languagesEndpoints.profilesLanguages}`, options)
      .pipe(
        tap(result => {
          const currentProfile = this.profileSubject.value;
          if (currentProfile) {
            const updatedProfile: ProfileDto = {
              ...currentProfile,
              languages: result
            };
            this.profileSubject.next(updatedProfile);
          }
        }),
      );
  }

  getAllInterests(): Observable<InterestDto[]> {
    return this.httpClient.get<InterestDto[]>(`${interestsEndpoints.interests}`, this.httpOptions)
      .pipe(
      );
  }

  addInterestToProfile(model: AddInterestToProfileDto): Observable<InterestDto[]> {
    return this.httpClient.post<InterestDto[]>(`${interestsEndpoints.profilesInterests}`, model, this.httpOptions)
      .pipe(
        tap(result => {
          const currentProfile = this.profileSubject.value;

          if (currentProfile) {
            const updatedProfile: ProfileDto = {
              ...currentProfile,
              interests: result
            };
            this.profileSubject.next(updatedProfile);
          }
        }),
      );
  }

  removeInterestFromProfile(profileId: number, interestId: number): Observable<InterestDto[]> {
    const options = {
      headers: this.httpOptions.headers,
      body: {
        profileId: profileId,
        interestId: interestId
      },
    };

    return this.httpClient.delete<InterestDto[]>(`${interestsEndpoints.profilesInterests}`, options)
      .pipe(
        tap(result => {
          const currentProfile = this.profileSubject.value;

          if (currentProfile) {
            const updatedProfile: ProfileDto = {
              ...currentProfile,
              interests: result
            };
            this.profileSubject.next(updatedProfile);
          }
        }),
      );
  }

  getAllGoals(): Observable<GoalDto[]> {
    return this.httpClient.get<GoalDto[]>(`${goalsEndpoints.goals}`, this.httpOptions)
      .pipe(
      );
  }

  getCitiesByCountryId(countryId: number): Observable<CityDto[]> {
    return this.httpClient.get<Array<CityDto>>(`${countriesEndpoints.cities(countryId)}`, this.httpOptions)
      .pipe(
      );
  }

  getAllEducations(): Observable<EducationDto[]> {
    return this.httpClient.get<EducationDto[]>(`${educationEndpoints.educations}`, this.httpOptions)
      .pipe(
      );
  }

  addEducationToProfile(model: AddEducationToProfileDto): Observable<ProfileEducationDto[]> {
    return this.httpClient.post<ProfileEducationDto[]>(`${educationEndpoints.profilesEducation}`, model, this.httpOptions)
      .pipe(
        tap(result => {
          const currentProfile = this.profileSubject.value;

          if (currentProfile) {
            const updatedProfile: ProfileDto = {
              ...currentProfile,
              education: result
            };
            this.profileSubject.next(updatedProfile);
          }
        }),
      );
  }

  updateProfileEducation(model: UpdateProfileEducationDto): Observable<ProfileEducationDto[]> {
    return this.httpClient.put<ProfileEducationDto[]>(`${educationEndpoints.profilesEducation}`, model, this.httpOptions)
      .pipe(
        tap(result => {
          const currentProfile = this.profileSubject.value;

          if (currentProfile) {
            const updatedProfile: ProfileDto = {
              ...currentProfile,
              education: result
            };
            this.profileSubject.next(updatedProfile);
          }
        }),
      );
  }

  removeEducationFromProfile(profileId: number, educationId: number): Observable<ProfileEducationDto[]> {
    const options = {
      headers: this.httpOptions.headers,
      body: {
        profileId: profileId,
        educationId: educationId
      },
    };

    return this.httpClient.delete<ProfileEducationDto[]>(`${educationEndpoints.profilesEducation}`, options)
      .pipe(
        tap(result => {
          const currentProfile = this.profileSubject.value;

          if (currentProfile) {
            const updatedProfile: ProfileDto = {
              ...currentProfile,
              education: result
            };
            this.profileSubject.next(updatedProfile);
          }
        }),
      );
  }

  addImage(model: AddImageDto): Observable<ImageDto> {
    const formData = new FormData();
    formData.append('file', model.file);

    return this.httpClient.post<ImageDto>(`${imagesEndpoints.images}?ProfileId=${model.profileId}`, formData)
      .pipe(
        tap(result => {
          let currentProfile = this.profileSubject.value;

          if(currentProfile){
            currentProfile.images.push(result);
            this.profileSubject.next(currentProfile);
          }
        }),
      );
  }

  changeMainImage(model: ChangeMainImageDto): Observable<ImageDto[]> {
    return this.httpClient.patch<ImageDto[]>(`${imagesEndpoints.images}`, model, this.httpOptions)
      .pipe(
        tap(result => {
          let currentProfile = this.profileSubject.value;

          if(currentProfile){
            currentProfile.images = result;
          }
        }),
      );
  }

  removeImage(profileId: number, imageId: number): Observable<ImageDto> {
    const options = {
      headers: this.httpOptions.headers,
      body: {
        profileId: profileId,
        imageId: imageId
      },
    };

    return this.httpClient.delete<ImageDto>(`${imagesEndpoints.images}`, options)
      .pipe(
        tap(() => {
          let currentProfile = this.profileSubject.value;

          if (currentProfile) {
            currentProfile.images = currentProfile.images.filter(image => image.id !== imageId);
            this.profileSubject.next(currentProfile);
          }
        }),
      );
  }
}
