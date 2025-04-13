import {Gender} from "../../constants/gender";

export interface UpdateProfileDto {
  id: string;
  name: string;
  lastName?: string;
  birthDate: Date;
  gender: Gender;
  bio?: string;
  height?: number;
  showAge: boolean;
  ageFrom: number;
  ageTo: number;
  maxDistance: number;
  preferredGender: Gender;
  goalId?: number;
  cityId: number;
  userId: string;
}
