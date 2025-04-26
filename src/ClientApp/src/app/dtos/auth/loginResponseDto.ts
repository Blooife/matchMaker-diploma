import {UserResponseDto} from "./userResponseDto";

export interface LoginResponseDto {
  id: number;
  email: string;
  refreshToken: string | null;
  refreshTokenExpiredAt: string | null;
  jwtToken: string;
}
