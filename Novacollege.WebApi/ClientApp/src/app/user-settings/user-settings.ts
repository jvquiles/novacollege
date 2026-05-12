import { Component } from '@angular/core';

@Component({
  selector: 'app-user-settings',
  standalone: true,
  imports: [],
  template: `
    <div class="settings-container">
      <h1>User Settings</h1>
      <p>Comunicación padre-hijo</p>
    </div>
  `,
  styles: [`
    .settings-container {
      padding: 2rem;
    }
    h1 {
      color: #333;
    }
  `]
})
export class UserSettingsComponent {}
