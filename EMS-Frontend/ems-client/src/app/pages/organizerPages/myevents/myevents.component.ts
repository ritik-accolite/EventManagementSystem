import { Component, OnInit } from '@angular/core';
import { UserdataService } from '../../../services/userDataService/userdata.service';
import { CommonModule, DatePipe, NgFor, NgIf } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { JwtDecodeService } from '../../../services/jwtDecodeService/jwtDecode.service';
import { OrganizerEventInterface } from '../../../interface/organizerInterface/organizer-event-interface';

@Component({
  selector: 'app-myevents',
  standalone: true,
  imports: [NgFor, NgIf, DatePipe, CommonModule],
  templateUrl: './myevents.component.html',
  styleUrls: ['./myevents.component.css'],
})
export class MyeventsComponent implements OnInit {
  events: any[] = [];
  role: any;
  organizerId: string = '';

  constructor(
    private userdataservice: UserdataService,
    private router: Router,
    private jwtDecodeService: JwtDecodeService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.role = localStorage.getItem('Role');
    this.route.params.subscribe((params) => {
      this.organizerId = this.jwtDecodeService.organizerId;
    });
    this.fetchEvents();
  }

  editEvent(eventId: number) {
    this.userdataservice.eventId = eventId;
    this.router.navigate(['organizer-dash', 'app-editevent']);
  }

  viewEvent(eventId: number) {
    this.userdataservice.eventId = eventId;
    if (this.jwtDecodeService.role === 'Organizer') {
      this.router.navigate(['organizer-dash', 'app-viewevent']);
    } else if (this.jwtDecodeService.role === 'Admin') {
      this.router.navigate(['admin-dash', 'app-viewevent']);
    }
  }

  truncateDescription(description: string, maxLength: number): string {
    if (description.length <= maxLength) {
      return description;
    } else {
      return description.substring(0, maxLength) + '...';
    }
  }

  fetchEvents(): void {
    if (this.organizerId != '') {
      this.userdataservice.getOrganizerEventsById(this.organizerId).subscribe(
        (response: OrganizerEventInterface) => {
          this.events = response.allEvents;
          this.sortEventsByDate();
        },
        (error) => console.error('Error fetching events: ', error)
      );
    } else {
      this.userdataservice.getOrganizerEvents().subscribe(
        (response: OrganizerEventInterface) => {
          this.events = response.allEvents;
          this.sortEventsByDate();
        },
        (error) => console.error('Error fetching events: ', error)
      );
    }
  }

  sortEventsByDate(): void {
    this.events.sort((a, b) => {
      return new Date(a.eventDate).getTime() - new Date(b.eventDate).getTime();
    });
  }

  isEditable(eventDate: string): boolean {
    const currentDate = new Date();
    const selectedDate = new Date(eventDate);
    const diffInDays = (selectedDate.getTime() - currentDate.getTime()) / (1000 * 3600 * 24);
    return diffInDays > 3;
  }

  isEventDone(eventDate: string): boolean {
    const currentDate = new Date();
    const selectedDate = new Date(eventDate);
    return selectedDate < currentDate;
  }
}
