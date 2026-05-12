import { ApplicationConfig, provideBrowserGlobalErrorListeners, Component } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';
import { UserSettingsComponent } from './user-settings/user-settings';
import { UsersComponent } from './users/users';

@Component({
  selector: 'app-home',
  standalone: true,
  template: `
    <div class="home">
      <h1>Welcome to NovaCollege!</h1>
      <p>Your Angular application is running.</p>
    </div>
  `,
  styles: [`
    .home { padding: 2rem; }
    h1 { color: #333; }
  `]
})
export class HomeComponent { }

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideHttpClient(),
    provideRouter([
      { path: '', component: HomeComponent },
      { path: 'user-settings', component: UserSettingsComponent },
      { path: 'users', component: UsersComponent }
    ])
  ]
};