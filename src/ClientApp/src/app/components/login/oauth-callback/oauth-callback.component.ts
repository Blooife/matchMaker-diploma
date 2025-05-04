import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import {environment} from "../../../constants/environment";
import {AuthService} from "../../../services/auth-service.service";

@Component({
  selector: 'app-oauth-callback',
  styleUrls: ['./oauth-callback.component.css'],
  templateUrl: './oauth-callback.component.html',
  standalone: true
})
export class OauthCallbackComponent implements OnInit {

  constructor(
    private route: ActivatedRoute,
    private http: HttpClient,
    private router: Router,
    private authService: AuthService,
  ) {}

  async ngOnInit() {
    const code = this.route.snapshot.queryParamMap.get('code');
    if (!code) return;
    try {
      const body = new HttpParams()
        .set('code', code)
        .set('client_id', environment.googleClientId)
        .set('client_secret', environment.googleClientSecret)
        .set('redirect_uri', environment.redirectUri)
        .set('grant_type', 'authorization_code');

      const tokenResponse: any = await this.http.post(
        'https://oauth2.googleapis.com/token',
        body.toString(),
        {
          headers: new HttpHeaders({ 'Content-Type': 'application/x-www-form-urlencoded' })
        }
      ).toPromise();

      const accessToken = tokenResponse.access_token;
      this.authService.loginWithGoogle(accessToken).subscribe({
        next: (isLoggedIn) => {
          if (isLoggedIn) {
            this.router.navigate(['']);
          }
        },
      });
    } catch (error) {
      console.error('Ошибка при авторизации через Google', error);
    }
  }
}
