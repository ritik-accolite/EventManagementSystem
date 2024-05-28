import { Component } from '@angular/core';

@Component({
  selector: 'app-aboutus',
  standalone: true,
  imports: [],
  templateUrl: './aboutus.component.html',
  styleUrls: ['./aboutus.component.css'],
})
export class AboutusComponent {
  showNext(): void {
    console.log('inside');
    let next = document.querySelector('.next') as HTMLElement;
    next.addEventListener('click', () => {
      let items = document.querySelectorAll('.item');
      let slider = document.querySelector('.slide') as HTMLElement;
      slider.appendChild(items[0]);
    });
  }

  showPrev(): void {
    console.log('hi');
    let prev = document.querySelector('.prev') as HTMLElement;
    prev.addEventListener('click', function () {
      let items = document.querySelectorAll('.item');
      let doc = document.querySelector('.slide') as HTMLElement;
      doc.prepend(items[items.length - 1]); // here the length of items = 6
    });
  }
}
