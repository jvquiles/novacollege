import { Component } from '@angular/core';
import { UserInfoComponent, UserInfo } from '../user-info/user-info';

@Component({
  selector: 'app-user-settings',
  standalone: true,
  imports: [UserInfoComponent],
  template: `
    <div class="settings-container">
      <h1>User Settings</h1>
      <app-user-info [userInfo]="detailView"/>
      <button (click)="increase()">Increase age</button>
      <button (click)="decrease()">Decrease age</button>
    </div>
  `,
  styles: [`
    .settings-container {
      padding: 2rem;
    }
    h1 {
      color: #333;
    }
    button {
      margin-top: 1rem;
      padding: 0.5rem 1rem;
      cursor: pointer;
    }
  `]
})
export class UserSettingsComponent {
  detailView = new UserInfo();

  increase() {
    this.detailView.age++;
  }

  decrease() {
    this.detailView.age--;
  }
}
