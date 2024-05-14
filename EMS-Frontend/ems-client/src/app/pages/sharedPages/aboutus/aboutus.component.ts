import { Component } from '@angular/core';

@Component({
  selector: 'app-aboutus',
  standalone: true,
  imports: [],
  templateUrl: './aboutus.component.html',
  styleUrls: ['./aboutus.component.css']
})
export class AboutusComponent {
  
  constructor() {
    this.init();
  }

  private init(): void {
    const next: HTMLButtonElement | null = document.querySelector('.next');
    const prev: HTMLButtonElement | null = document.querySelector('.prev');

    if (next && prev) {
      next.addEventListener('click', () => {
        const items: NodeListOf<HTMLElement> = document.querySelectorAll('.item');
        const slide: HTMLElement | null = document.querySelector('.slide');
        if (slide) {
          slide.appendChild(items[0]);
        }
      });

      prev.addEventListener('click', () => {
        const items: NodeListOf<HTMLElement> = document.querySelectorAll('.item');
        const slide: HTMLElement | null = document.querySelector('.slide');
        if (slide) {
          slide.prepend(items[items.length - 1]);
        }
      });
    }
  }

}