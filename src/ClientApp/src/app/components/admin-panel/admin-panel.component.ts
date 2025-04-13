import {Component, OnDestroy, OnInit} from '@angular/core';
import { UserResponseDto } from "../../dtos/auth/userResponseDto";
import { UserService } from "../../services/user-service.service";
import { AssignRoleRequestDto } from "../../dtos/auth/assignRoleRequestDto";
import {AsyncPipe, CommonModule, NgForOf, NgIf} from "@angular/common";
import { roles } from '../../constants/roles';
import { FormsModule } from "@angular/forms";
import { AuthService } from "../../services/auth-service.service";
import {Subscription} from "rxjs";

@Component({
  selector: 'app-admin-panel',
  templateUrl: './admin-panel.component.html',
  styleUrls: ['./admin-panel.component.css'],
  imports: [
    CommonModule,
    FormsModule,
    AsyncPipe
  ],
  standalone: true
})
export class AdminPanelComponent implements OnInit, OnDestroy {
  users$: UserResponseDto[] = [];
  pageSize: number = 6;
  pageNumber: number = 1;
  totalPages: number = 0;
  roles = Object.values(roles);
  selectedRole: string = '';
  private subscriptions: Subscription[] = [];

  constructor(private userService: UserService, private authService: AuthService) {

  }

  ngOnInit(): void {
    const sub = this.userService.users$.subscribe((users)=>{
        this.users$ = users;
      }
    );
    this.subscriptions.push(sub);
    this.loadUsers();
  }

  ngOnDestroy() {
    this.subscriptions.forEach(sub=>sub.unsubscribe());
  }

  loadUsers() {
    this.userService.getPaginatedUsers(this.pageSize, this.pageNumber).subscribe({
      next: (result) => {
        this.totalPages = result.pagination.TotalPages;
    }
    });
  }

  deleteUser(userId: string) {
    this.userService.deleteUserById(userId).subscribe({
      next: () => {
        this.loadUsers();
      }
    });
  }

  assignRole(email: string, role: string) {
    const model: AssignRoleRequestDto = { email, role };
    this.userService.assignRole(model).subscribe({
      next: () => {
        this.loadUsers();
      }
    });
  }

  removeRole(email: string, role: string) {
    this.userService.removeFromRole(email, role).subscribe({
      next: () => {
        this.loadUsers();
      }
    });
  }

  nextPage() {
    if (this.pageNumber < this.totalPages) {
      this.pageNumber++;
      this.loadUsers();
    }
  }

  previousPage() {
    if (this.pageNumber > 1) {
      this.pageNumber--;
      this.loadUsers();
    }
  }

  isAdmin() {
    return this.authService.checkRights(roles.admin);
  }
}
