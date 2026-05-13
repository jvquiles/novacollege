import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';

interface User {  
  name: string;
  age: number;
}

// Utilizar directivas estructurales de Angular para mostrar el nombre de cada personaje y si es
// mayor de edad
@Component({
  selector: 'app-users',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="users-container">
      <h1>Users</h1>
      <button (click)="loadUsers()">Load Users</button>      
      <ul>
        @for (user of users(); track user.name) {
          <li>{{user.name}} - 
            @if (user.age >= 18) {
              <span>Mayor de edad</span>
            } @else {
              <span>Menor de edad</span>
            }
          </li>
        }
      </ul>
    </div>
  `,
  styles: [`
    .users-container {
      padding: 2rem;
    }
    h1 {
      color: #333;
    }
    button {
      margin: 1rem 0;
      padding: 0.5rem 1rem;
    }
    ul {
      list-style: none;
    }
    li {
      padding: 0.5rem;
      border-bottom: 1px solid #ddd;
    }
  `]
})
export class UsersComponent implements OnInit {
  users = signal<User[]>([]);

  constructor(private http: HttpClient) {}

  ngOnInit() { }

  loadUsers() {
    this.http.get<User[]>('https://6390b47b65ff4183111c4b91.mockapi.io/users/users')
      .subscribe({
        next: (data) => {
          this.users.set(data);
        },
        error: (err) => {
          console.log(`Failed to load users: ${err}`);
        }
      });
  }
}
