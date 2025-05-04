import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders} from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import {map, retry, tap} from 'rxjs/operators';
import { GeneralResponseDto } from "../dtos/shared/generalResponseDto";
import { UserResponseDto } from "../dtos/auth/userResponseDto";
import { AssignRoleRequestDto } from "../dtos/auth/assignRoleRequestDto";
import {reportsEndpoints, rolesEndpoints, usersEndpoints} from "../constants/api-endpoints";
import {ReportTypeDto} from "../dtos/report/ReportTypeDto";
import {ModerateReportDto} from "../dtos/report/ModerateReportDto";
import {CreateUserReportDto} from "../dtos/report/CreateUserReportDto";
import {ReportFilterDto} from "../dtos/report/ReportFilterDto";
import {UserReportDto} from "../dtos/report/UserReportDto";

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
    return this.httpClient.get<UserResponseDto[]>(
      `${usersEndpoints.paginatedUsers(pageSize.toString(), pageNumber.toString())}`,
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

  deleteUserById(userId: number): Observable<GeneralResponseDto> {
    return this.httpClient.delete<GeneralResponseDto>(
      `${usersEndpoints.users}/${userId}`, this.httpOptions).pipe(
    );
  }

  assignRole(model: AssignRoleRequestDto): Observable<GeneralResponseDto> {
    return this.httpClient.post<GeneralResponseDto>(
      `${rolesEndpoints.assignment}`, model, this.httpOptions).pipe(
    );
  }

  removeFromRole(email: string, role: string): Observable<GeneralResponseDto> {
        const model = {
      email, role
    };

    return this.httpClient.post<GeneralResponseDto>(
      `${rolesEndpoints.removal}`,model, this.httpOptions).pipe(
    );
  }

  getPaginatedReports(filter: ReportFilterDto, pageSize: number, pageNumber: number): Observable<{ reports: UserReportDto[], pagination: any }> {
    return this.httpClient.post<UserReportDto[]>(
      `${reportsEndpoints.reports}/paginated?pageSize=${pageSize}&pageNumber=${pageNumber}`,
      filter,
      {
        ...this.httpOptions,
        observe: 'response'
      }
    ).pipe(
      map(response => {
        const pagination = JSON.parse(response.headers.get('X-Pagination')!);
        return {
          reports: response.body || [],
          pagination: pagination
        };
      })
    );
  }

  createReport(model: CreateUserReportDto): Observable<void> {
    return this.httpClient.post<void>(`${reportsEndpoints.reports}`, model, {...this.httpOptions.headers});
  }

  moderateReport(dto: ModerateReportDto): Observable<void> {
    return this.httpClient.post<void>(`${reportsEndpoints.reports}/moderate`, dto, {...this.httpOptions.headers});
  }

  getReportTypes(): Observable<ReportTypeDto[]> {
    return this.httpClient.get<ReportTypeDto[]>(`${reportsEndpoints.reports}/types`, {...this.httpOptions.headers});
  }
}
