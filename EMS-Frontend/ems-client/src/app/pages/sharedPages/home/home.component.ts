import { NgIf, CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { Router, RouterOutlet, RouterLink } from '@angular/router';
import { EventlistComponent } from '../eventlist/eventlist.component';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RouterOutlet, NgIf, RouterLink, EventlistComponent, CommonModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class HomeComponent {
  constructor() {}
}
