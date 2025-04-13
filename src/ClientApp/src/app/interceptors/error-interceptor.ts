import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ErrorHandlerService } from '../services/error-handler.service';
import { _IGNORED_STATUSES } from '../constants/http-context';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(private errorHandlerService: ErrorHandlerService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const ignoredStatuses = req.context.get(_IGNORED_STATUSES);
    return next.handle(req).pipe(
      catchError((error: HttpErrorResponse) => {
        if (ignoredStatuses || error.status === 401) {
          return throwError(() => error);
        }

        // Теперь ошибки обрабатываются через ErrorHandlerService
        this.errorHandlerService.handleError(error);

        return throwError(() => error);
      })
    );
  }
}
