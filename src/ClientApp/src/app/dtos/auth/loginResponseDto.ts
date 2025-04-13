import {UserResponseDto} from "./userResponseDto";

export interface LoginResponseDto {
  id: string;
  email: string;
  refreshToken: string | null;
  refreshTokenExpiredAt: string | null;
  jwtToken: string;
}
