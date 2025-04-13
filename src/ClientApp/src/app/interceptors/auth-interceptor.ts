import { Injectable } from '@angular/core';
import {
  HttpEvent,
  HttpInterceptor,
  HttpHandler,
  HttpRequest,
  HttpErrorResponse,
} from '@angular/common/http';
import { Observable, throwError, BehaviorSubject } from 'rxjs';
import { catchError, filter, switchMap, take } from 'rxjs/operators';
import { AuthService } from '../services/auth-service.service';
import { Router } from '@angular/router';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  private isRefreshing = false;
  private refreshTokenSubject: BehaviorSubject<any> = new BehaviorSubject<any>(null);

  constructor(private authService: AuthService, private router: Router) {}

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    let accessToken = this.authService.getAccessToken();
    console.log("intercept", req)
    if (accessToken) {
      req = this.setAuthorizationHeader(req, accessToken);
    }

    return next.handle(req).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 401) {
          return this.handle401Error(req, next);
        }
        return throwError(() => error);
      })
    );
  }

  private handle401Error(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (!this.isRefreshing) {
      this.isRefreshing = true;
      this.refreshTokenSubject.next(null);

      return this.authService.refreshToken().pipe(
        switchMap((isRefreshed: boolean) => {
          this.isRefreshing = false;
          if (isRefreshed) {
            const accessToken = this.authService.getAccessToken();
            this.refreshTokenSubject.next(accessToken);
            req = this.setAuthorizationHeader(req, accessToken!);
            return next.handle(req);
          } else {
            this.authService.logOut();
            this.goToLogIn();
            return throwError(() => new Error('Token refresh failed'));
          }
        }),
        catchError((err) => {
          this.isRefreshing = false;
          this.authService.logOut();
          this.goToLogIn();
          return throwError(() => err);
        })
      );
    } else {
      return this.refreshTokenSubject.pipe(
        filter(token => token != null),
        take(1),
        switchMap(accessToken => {
          req = this.setAuthorizationHeader(req, accessToken);
          return next.handle(req);
        })
      );
    }
  }

  setAuthorizationHeader(
    req: HttpRequest<any>,
    accessToken: string
  ): HttpRequest<any> {
    return req.clone({
      setHeaders: {
        Authorization: `Bearer ${accessToken}`,
      },
    });
  }

  private goToLogIn() {
    this.router.navigate(['']);
  }
}
