import { Component } from '@angular/core';
import { UserdataService } from '../../../services/userDataService/userdata.service';
import { NgFor, NgIf, DatePipe } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-trackevent',
  standalone: true,
  imports: [NgFor, NgIf, DatePipe],
  templateUrl: './trackevent.component.html',
  styleUrl: './trackevent.component.css',
})
export class TrackeventComponent {
  events: any[] = [];
  constructor(
    private userdataservice: UserdataService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.fetchEvents();
  }
  fetchEvents(): void {
    this.userdataservice.getEvents().subscribe(
      (response: any) => {
        this.events = response.allEvents;
      },
      (error) => console.error('Error fetching events: ', error)
    );
  }
  onSelectEvent(eventId: number): void {
    this.userdataservice.eventId = eventId;
    this.router.navigate(['admin-dash', 'app-viewevent']);
  }
}
