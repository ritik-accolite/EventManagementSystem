import { Component } from '@angular/core';
import { Location } from '@angular/common';

@Component({
  selector: 'unauthorised',
  standalone: true,
  imports: [],
  templateUrl: './unauthorised.component.html',
  styleUrl: './unauthorised.component.css'
})
export class UnauthorisedComponent {
  constructor(private location: Location) {}

  goBack(): void {
    this.location.back();
    this.location.back();
  }
}
