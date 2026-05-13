import { Component } from '@angular/core';
import { UserInfoComponent, UserInfo } from '../user-info/user-info';

@Component({
  selector: 'app-user-settings',
  standalone: true,
  imports: [UserInfoComponent],
  template: `
    <div class="settings-container">
      <h1>User Settings</h1>
      <app-user-info [(userInfo)]="detailView"/>
      <button (click)="increaseAge()">Increase age from parent</button>
      <button (click)="decreaseAge()">Decrease age from parent</button>
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
      margin-right: 0.5rem;
      padding: 0.5rem 1rem;
      cursor: pointer;
    }
  `]
})
export class UserSettingsComponent {
  private _detailView = new UserInfo();

  get detailView(): UserInfo {
    return this._detailView;
  }

  set detailView(value: UserInfo) {
    this._detailView = value;
    this.onDetailViewChanged(value);
  }

  // Utilizando los componentes implementados en el ejercicio 1, crear una función en el
  // componente hijo para incrementar la edad de un usuario. Mostrar la nueva edad en el
  // componente padre mediante un console.log.
  private onDetailViewChanged(value: UserInfo) {
    console.log('detailView changed from child:', value);
  }

  increaseAge() {
    this._detailView.age++;
  }

  decreaseAge() {
    this._detailView.age--;
  }
}
