import { Component, OnInit } from '@angular/core';
import { UserdataService } from '../../../services/userDataService/userdata.service';
import { NgFor, NgIf, DatePipe, CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { FormControl, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-trackevent',
  standalone: true,
  imports: [NgFor, NgIf, DatePipe, ReactiveFormsModule, CommonModule],
  templateUrl: './trackevent.component.html',
  styleUrls: ['./trackevent.component.css'],
})
export class TrackeventComponent implements OnInit {
  events: any[] = [];
  filteredEvents: any[] = [];
  searchControl = new FormControl('');

  constructor(
    private userdataservice: UserdataService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.fetchEvents();
    this.searchControl.valueChanges.subscribe((searchTerm) => {
      this.applyFilter(searchTerm);
    });
  }

  fetchEvents(): void {
    this.userdataservice.getEvents().subscribe(
      (response: any) => {
        this.events = response.allEvents.sort((a: any, b: any) => new Date(a.eventDate).getTime() - new Date(b.eventDate).getTime());
        this.filteredEvents = this.events; // Initialize filtered events
      },
      (error) => console.error('Error fetching events: ', error)
    );
  }

  applyFilter(searchTerm: any): void {
    const lowerCaseSearchTerm = searchTerm.toLowerCase();
    this.filteredEvents = this.events.filter((event) =>
      `${event.organizerFirstName} ${event.organizerLastName}`.toLowerCase().includes(lowerCaseSearchTerm) ||
      event.eventName.toLowerCase().includes(lowerCaseSearchTerm) ||
      event.eventCategory.toLowerCase().includes(lowerCaseSearchTerm)
    );
  }

  onSelectEvent(eventId: number): void {
    this.userdataservice.eventId = eventId;
    this.router.navigate(['admin-dash', 'app-viewevent']);
  }

  isEventDone(eventDate: string): boolean {
    const currentDate = new Date();
    const selectedDate = new Date(eventDate);
    return selectedDate < currentDate;
  }
}
