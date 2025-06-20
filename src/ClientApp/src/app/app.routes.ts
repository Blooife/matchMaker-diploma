import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import {RegisterComponent} from "./components/register/register.component";
import {HomeComponent} from "./components/home/home.component";
import {AdminPanelComponent} from "./components/admin-panel/admin-panel.component";
import {ProfileComponent} from "./components/profile/profile.component";
import {CreateProfileComponent} from "./components/profile/create-profile/create-profile.component";
import {UpdateProfileComponent} from "./components/profile/update-profile/update-profile.component";
import {AdminGuard} from "./guards/admin-guard";
import {LoggedInGuard} from "./guards/loggedIn-guard";
import {FindMatchComponent} from "./components/find-match/find-match.component";
import {MatchesComponent} from "./components/matches/matches.component";
import {ProfileCreatedGuard} from "./guards/profileCreated-guard";
import {ChatsComponent} from "./components/chats/chats.component";
import {SettingsComponent} from "./components/settings/settings.component";
import {BlackListComponent} from "./components/black-list/black-list.component";
import {ModeratorPanelComponent} from "./components/moderator-panel/moderator-panel.component";
import {OauthCallbackComponent} from "./components/login/oauth-callback/oauth-callback.component";
import {ChangePasswordComponent} from "./components/settings/change-password/change-password.component";

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'admin-panel', component: AdminPanelComponent, canActivate: [AdminGuard] },
  { path: 'moderator-panel', component: ModeratorPanelComponent, canActivate: [AdminGuard] },
  { path: 'profile', component: ProfileComponent, canActivate: [ProfileCreatedGuard] },
  { path: 'create-profile/:userId', component: CreateProfileComponent },
  { path: 'update-profile/:profileId', component: UpdateProfileComponent, canActivate: [ProfileCreatedGuard] },
  { path: 'find-match', component: FindMatchComponent, canActivate: [ProfileCreatedGuard] },
  { path: 'matches', component: MatchesComponent, canActivate: [ProfileCreatedGuard] },
  { path: 'chats', component: ChatsComponent, canActivate: [ProfileCreatedGuard] },
  { path: 'home', component: HomeComponent, canActivate: [LoggedInGuard] },
  { path: 'settings', component: SettingsComponent, canActivate: [ProfileCreatedGuard] },
  { path: 'black-list', component: BlackListComponent, canActivate: [ProfileCreatedGuard] },
  { path: 'change-password', component: ChangePasswordComponent },
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'oauth-callback', component: OauthCallbackComponent }
];
