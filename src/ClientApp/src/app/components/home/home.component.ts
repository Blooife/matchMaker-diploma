import {Component} from '@angular/core';
import {RouterLink} from '@angular/router';
import {AuthService} from "../../services/auth-service.service";
import {NgIf} from "@angular/common";
import {roles} from "../../constants/roles";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
  imports: [
    NgIf,
    RouterLink
  ],
  standalone: true
})
export class HomeComponent{

  constructor(private authService: AuthService) {

  }

  isLoggedIn() {
    return this.authService.isLogged();
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
}
