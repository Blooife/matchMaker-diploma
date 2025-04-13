import {Gender} from "../../constants/gender";
import {LanguageDto} from "../language/LanguageDto";
import {InterestDto} from "../interest/InterestDto";
import {ProfileEducationDto} from "../education/ProfileEducationDto";
import {ImageDto} from "../image/ImageDto";
import {GoalDto} from "../goal/GoalDto";
import {CityDto} from "../city/CityDto";
import {CountryDto} from "../country/CountryDto";

export interface ProfileDto {
  id: string;
  name: string;
  lastName: string;
  birthDate: Date;
  bio?: string;
  height?: number;
  showAge: boolean;
  ageFrom: number;
  ageTo: number;
  gender: Gender;
  preferredGender: Gender;
  maxDistance: number;
  city: CityDto;
  country: CountryDto;
  goal?: GoalDto;
  userId: string;
  languages: LanguageDto[];
  interests: InterestDto[];
  education: ProfileEducationDto[];
  images: ImageDto[];
}
