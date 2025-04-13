import {Component, OnDestroy, OnInit} from '@angular/core';
import {Router, RouterLink} from '@angular/router';
import {AuthService} from "../../services/auth-service.service";
import {AsyncPipe, NgIf} from "@angular/common";
import {Subscription} from "rxjs";
import {roles} from "../../constants/roles";
import {UserService} from "../../services/user-service.service";

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
  imports: [
    NgIf,
    RouterLink,
    AsyncPipe
  ],
  standalone: true
})
export class HeaderComponent implements OnInit, OnDestroy {
  isLoggedIn$: boolean = false;
  currentUserId$: string | null = '';
  isMenuVisible = true;
  private subscriptions: Subscription[] = [];

  constructor(private authService: AuthService, private router: Router, private userService: UserService) {

  }

  ngOnInit() {
    const sub1 = this.authService.isLoggedIn$.subscribe((isLoggedIn) =>{
      this.isLoggedIn$ = isLoggedIn;
    });

    const sub2 = this.authService.currentUserId$.subscribe((id) =>{
      this.currentUserId$ = id;
    });

    this.subscriptions.push(sub1, sub2);
  }

  ngOnDestroy() {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }

  toggleMenu() {
    this.isMenuVisible = !this.isMenuVisible;
  }

  logOut() {
    this.authService.logOut();
    this.router.navigate(['']);
  }

  isAdmin(){
    return this.authService.checkRights(roles.admin);
  }

  isModerator(){
    return this.authService.checkRights(roles.moderator);
  }

  isUser(){
    return this.authService.checkRights(roles.user);
  }

  deleteUserById(){
    if(this.currentUserId$){
      if (confirm('Are you sure you want to delete your account?')) {
        this.userService.deleteUserById(this.currentUserId$).subscribe(
          () =>{
            this.authService.logOut();
            this.router.navigate(['']);
          }
        );
      }
    }
  }
}
