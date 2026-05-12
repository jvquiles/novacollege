import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-user-info',
  standalone: true,
  imports: [],
  template: `
    <div class="settings-container">
      <h2>User Info</h2>
      <p>{{ userInfo.name }} has ({{ userInfo.age }} years old)</p>
    </div>
  `,
  styles: [`
    .settings-container {
      padding: 2rem;
    }
    h2 {
      color: #666;
    }
  `]
})
export class UserInfoComponent {
  private _userInfo: UserInfo = new UserInfo();

  // ¿Cómo ejecutarías una función en el componente “user-info” cada vez que la variable
  // “detailView” cambie dentro del componente “user-settings”? 
  @Input()
  set userInfo(value: UserInfo) {
    if (value !== this._userInfo) {
      this._userInfo = value;
      this.onUserInfoChange();
    }
  }

  get userInfo(): UserInfo
  {
    return this._userInfo;
  }

  private onUserInfoChange() {
    console.log('userInfo changed to:', this._userInfo);
  }
}

export class UserInfo
{
  name: string = 'John Doe';
  age: number = 30;
}
