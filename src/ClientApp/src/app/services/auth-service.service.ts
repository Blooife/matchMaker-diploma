import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { UserRequestDto } from '../dtos/auth/userRequestDto';
import { GeneralResponseDto } from '../dtos/shared/generalResponseDto';
import { LoginResponseDto } from '../dtos/auth/loginResponseDto';
import { RefreshRequestDto } from '../dtos/auth/refreshRequestDto';
import { jwtDecode } from 'jwt-decode';
import { authEndpoints } from '../constants/api-endpoints';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  private isLoggedInSubject = new BehaviorSubject<boolean>(this.isLogged());
  isLoggedIn$ = this.isLoggedInSubject.asObservable();

  private currentUserIdSubject = new BehaviorSubject<string | null>(this.getCurrentUserId());
  currentUserId$ = this.currentUserIdSubject.asObservable();

  private currentUserRolesSubject = new BehaviorSubject<string[] | null>(this.getCurrentUserRoles());
  currentUserRoles$ = this.currentUserRolesSubject.asObservable();

  constructor(private httpClient: HttpClient) { }

  register(model: UserRequestDto): Observable<GeneralResponseDto> {
    return this.httpClient.post<GeneralResponseDto>(`${authEndpoints.register}`, model, this.httpOptions)
      .pipe(
      );
  }

  login(model: UserRequestDto): Observable<boolean> {
    return this.httpClient.post<LoginResponseDto>(`${authEndpoints.login}`, model, this.httpOptions)
      .pipe(
        tap(response => {
          if (response.refreshToken) {
            this.setTokens(response);
          } else {
            this.isLoggedInSubject.next(false);
            this.currentUserIdSubject.next(null);
            this.currentUserRolesSubject.next(null);
          }
        }),
        map(response => !!response.refreshToken),
      );
  }

  refreshToken(): Observable<boolean> {


    const refreshToken = this.getRefreshToken();
    console.log(refreshToken)
    if (!refreshToken) {
      return of(false);
    }

    const model: RefreshRequestDto = { refreshToken };

    return this.httpClient.post<LoginResponseDto>(`${authEndpoints.refresh}`, model, this.httpOptions)
      .pipe(
        tap(response => {
          if (response.refreshToken) {
            console.log(response)
            this.setTokens(response);
          } else {
            this.isLoggedInSubject.next(false);
            this.currentUserIdSubject.next(null);
            this.currentUserRolesSubject.next(null);
          }
        }),
        map(response => !!response.refreshToken),
      );
  }

  logOut() {
    localStorage.removeItem('access_token');
    localStorage.removeItem('refresh_token');
    localStorage.removeItem('access_expires_at');
    this.isLoggedInSubject.next(false);
    this.currentUserIdSubject.next(null);
    this.currentUserRolesSubject.next(null);
  }

  getAccessToken(): string | null {
    return localStorage.getItem('access_token');
  }

  getRefreshToken(): string | null {
    return localStorage.getItem('refresh_token');
  }

  getExpiresAt(): string | null {
    return localStorage.getItem('access_expires_at');
  }

  isLogged(): boolean {
    const expiration = this.getExpiresAt();

    if (expiration) {
      const expiresAt = new Date(JSON.parse(expiration));
      const currentTime = new Date(Date.now());

      return currentTime < expiresAt;
    }

    return false;
  }

  getCurrentUserId(): string | null {
    if (!this.isLogged()) {
      return null;
    }

    const accessToken = this.getAccessToken();

    if (accessToken) {
      const payload = jwtDecode<{ sub: string }>(accessToken);
      return payload.sub;
    }

    return null;
  }

  getCurrentUserRoles(): string[] | null {
    if (!this.isLogged()) {
      return null;
    }

    const accessToken = this.getAccessToken();

    if (accessToken) {
      const payload = jwtDecode<{ role: string[] }>(accessToken);
      return payload.role;
    }

    return null;
  }

  checkRights(role: string): boolean {
    const userRoles = this.getCurrentUserRoles();

    if (userRoles) {
      return userRoles.includes(role);
    }

    return false;
  }

  setTokens(loginDto: LoginResponseDto) {
    const payload = jwtDecode<{ exp: number }>(loginDto.jwtToken);

    if (payload.exp) {
      localStorage.setItem('access_token', loginDto.jwtToken);
      localStorage.setItem('refresh_token', loginDto.refreshToken!);
      localStorage.setItem('access_expires_at', JSON.stringify(new Date(payload.exp * 1000)));
    }

    this.isLoggedInSubject.next(true);
    this.currentUserIdSubject.next(this.getCurrentUserId());
    this.currentUserRolesSubject.next(this.getCurrentUserRoles());
  }
}
