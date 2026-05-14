import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { forkJoin, switchMap, map } from 'rxjs';

interface User {
  id: string;
  name: string;
  cityId: number;
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
      <button (click)="loadDataFilteringByCity()">Load users in cities filtering by city</button>
      
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
        const citizen = users.map(user => ({
          user,
          city: noviCity
        }));
        this.data.set(citizen);
      },
      error: (err) => {
        console.log('Failed to load data', err);
      }
    });
  }

  // En el caso de que necesitemos primero obtener el id de usuario para luego enviar una
  // consulta al mismo endpoint de cities para obtener las ciudades por id de usuario, ¿cómo lo
  // implementarías en tu código para primero llamar a una api y luego a otra con el resultado de la
  // primera?(cuando obtengas los usuarios, asume que todos viven en Belgium)
  loadDataFilteringByCity() {
  this.http.get<User[]>('https://646b8fc77d3c1cae4ce3ffe0.mockapi.io/commonapi/users')
      .pipe(
        switchMap(users => {
          const cityRequests = users.map(user =>
            this.http.get<City>(`https://646b8fc77d3c1cae4ce3ffe0.mockapi.io/commonapi/cities/${user.id}`) // WARNING: Using user id just for demo purposes, not suitable for business apps
              .pipe(
                map(city => ({
                  user,
                  city: city
                } as Citizen))
              )
          );
          return forkJoin(cityRequests);
        })
      )
      .subscribe({
        next: (citizens) => {
          this.data.set(citizens);
        },
        error: (err) => {
          console.log(`Failed: ${err}`);
        }
      });
  }
}
