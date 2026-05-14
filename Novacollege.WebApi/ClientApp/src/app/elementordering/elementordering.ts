import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';

// ¿Cómo harías, mediante css, para colocar el primer div debajo del segundo div ?
@Component({
  selector: 'app-elementordering',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="container">
      <h1>Element Ordering</h1>
      <button (click)="backToRegular()">First is firt</button>
      <button (click)="turnUpSideDown()">First is last</button>
      <div [class.regular-container]="isRegular" [class.upsidedown-container]="!isRegular">
        <div>
          <p> Soy el primer div </p>
        </div>
        <div>
          <p> Soy el segundo div </p>
        </div>
      </div>
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
    .regular-container {
      display: flex;
      flex-direction: column;
    }
    .upsidedown-container {
      display: flex;
      flex-direction: column-reverse;
    }
    .regular-container div, .upsidedown-container div {
      padding: 0.5rem;
      border-bottom: 1px solid #ddd;
    }
  `]
})
export class ElementOrderingComponent {
  isRegular: boolean = true;
  backToRegular() {
    this.isRegular = true;
  }

  turnUpSideDown() {
    this.isRegular = false;
  }
}
