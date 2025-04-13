import {ApplicationConfig, importProvidersFrom, provideZoneChangeDetection} from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import {HTTP_INTERCEPTORS, provideHttpClient, withInterceptorsFromDi} from "@angular/common/http";
import {AuthInterceptor} from "./interceptors/auth-interceptor";
import {ErrorInterceptor} from "./interceptors/error-interceptor";
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import {provideToastr, ToastrModule} from 'ngx-toastr';

export const appConfig: ApplicationConfig = {
  providers: [
    importProvidersFrom([BrowserAnimationsModule, ToastrModule.forRoot()]),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    [
      {
        provide: HTTP_INTERCEPTORS,
        useClass: AuthInterceptor,
        multi: true,
      },
      {
        provide: HTTP_INTERCEPTORS,
        useClass: ErrorInterceptor,
        multi: true,
      },
    ],
    provideHttpClient(
      withInterceptorsFromDi()
    ),
    provideAnimationsAsync(),
    provideToastr(),
  ]
};
