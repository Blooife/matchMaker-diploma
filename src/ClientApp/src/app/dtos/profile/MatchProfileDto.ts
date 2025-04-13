import {Gender} from "../../constants/gender";
import {LanguageDto} from "../language/LanguageDto";
import {InterestDto} from "../interest/InterestDto";
import {ImageDto} from "../image/ImageDto";

export interface MatchProfileDto {
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
  city: string;
  country: string;
  goal?: string;
  languages: LanguageDto[];
  interests: InterestDto[];
  images: ImageDto[];
}
