import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders} from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import {map, retry, tap} from 'rxjs/operators';
import { GeneralResponseDto } from "../dtos/shared/generalResponseDto";
import { UserResponseDto } from "../dtos/auth/userResponseDto";
import { AssignRoleRequestDto } from "../dtos/auth/assignRoleRequestDto";
import { rolesEndpoints, usersEndpoints } from "../constants/api-endpoints";

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  private usersSubject = new BehaviorSubject<UserResponseDto[]>([]);
  users$ = this.usersSubject.asObservable();

  constructor(private httpClient: HttpClient) { }

  getPaginatedUsers(pageSize: number, pageNumber: number): Observable<{ users: UserResponseDto[], pagination: any }> {

    return this.httpClient.get<UserResponseDto[]>(`${usersEndpoints.paginatedUsers(pageSize.toString(), pageNumber.toString())}`,
      {
        ...this.httpOptions,
        observe: 'response'
      }).pipe(
      map(response => {
        const pagination = JSON.parse(response.headers.get('X-Pagination')!);

        return {
          users: response.body || [],
          pagination: pagination
        };
      }),
      tap(result => {
        this.usersSubject.next(result.users);
      })
    );
  }

  deleteUserById(userId: string): Observable<GeneralResponseDto> {
    return this.httpClient.delete<GeneralResponseDto>(`${usersEndpoints.users}/${userId}`, this.httpOptions).pipe(
      retry(2),
    );
  }

  assignRole(model: AssignRoleRequestDto): Observable<GeneralResponseDto> {
    return this.httpClient.post<GeneralResponseDto>(`${rolesEndpoints.assignment}`, model, this.httpOptions).pipe(
      retry(2),
    );
  }

  removeFromRole(email: string, role: string): Observable<GeneralResponseDto> {
    const options = {
      headers: this.httpOptions.headers,
      body: {
        email,
        role
      },
    };

    return this.httpClient.delete<GeneralResponseDto>(`${rolesEndpoints.removal}`, options).pipe(
      retry(2),
    );
  }
}
