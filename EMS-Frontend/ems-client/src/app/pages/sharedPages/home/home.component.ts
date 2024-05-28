import { NgIf } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { Router, RouterOutlet, RouterLink } from '@angular/router';
import { EventlistComponent } from '../eventlist/eventlist.component';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RouterOutlet, NgIf, RouterLink, EventlistComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
})
export class HomeComponent {
  constructor() {}
}
