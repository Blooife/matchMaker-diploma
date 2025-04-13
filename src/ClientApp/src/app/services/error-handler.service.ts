import { Injectable, EventEmitter } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { ErrorDetails } from '../dtos/shared/ErrorDetails';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root',
})
export class ErrorHandlerService {
  errorOccurred = new EventEmitter<any>();

  constructor(private toastr: ToastrService) {}

  handleError(error: HttpErrorResponse): Observable<never> {
    let errorMessage;

    if (error.error instanceof ErrorEvent) {
      errorMessage = `${error.error.message}`;
    } else if (error.error) {
      const errorDetails: ErrorDetails = error.error;
      errorMessage = `${errorDetails.ErrorMessage}`;
    } else {
      errorMessage = `${error.message}`;
    }

    this.toastr.error(errorMessage, 'Ошибка!', {
      timeOut: 2000,
      positionClass: 'toast-top-right',
      closeButton: true,
      progressBar: true,
    });

    //this.errorOccurred.emit({ message: errorMessage, status: error.status });

    return throwError(errorMessage);
  }
}
