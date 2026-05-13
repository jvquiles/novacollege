import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { forkJoin } from 'rxjs';

interface User {
  id: string;
  name: string;
}

interface City {
  id: number;
  name: string;
  country: string;
  countrycode: string;
}

interface Citizen {
  user: User;
  city: City | undefined;
}

@Component({
  selector: 'app-citizens',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="container">
      <h1>Citizens</h1>
      <button (click)="loadData()">Load users in cities</button>
      
      @if (data()) {
        <ul>
          @for (citizen of data()!; track citizen.user.id) {
            <li>
              {{ citizen.user.name }}
              @if (citizen.city != undefined) {
                lives in {{ citizen.city.name }} ({{ citizen.city.country }})
              }
            </li>
          }
        </ul>
      }
    </div>
  `,
  styles: [`
    .container {
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
export class CitizensComponent {
  data = signal<Citizen[]>([]);

  constructor(private http: HttpClient) {}

  loadData() {
    const users$ = this.http.get<User[]>('https://646b8fc77d3c1cae4ce3ffe0.mockapi.io/commonapi/users');
    const cities$ = this.http.get<City[]>('https://646b8fc77d3c1cae4ce3ffe0.mockapi.io/commonapi/cities');

    // ¿Cómo lo tenemos que hacer, con rxjs, para obtener los datos de ambos endpoints de esta API
    // al mismo tiempo ?
    forkJoin([users$, cities$]).subscribe({
      next: ([users, cities]) => {
        const noviCity = cities.find(c => c.country === 'Belgium');
        const citizens = users.map(user => ({
          user,
          city: noviCity
        }));
        this.data.set(citizens);
      },
      error: (err) => {
        console.log('Failed to load data', err);
      }
    });
  }
}
