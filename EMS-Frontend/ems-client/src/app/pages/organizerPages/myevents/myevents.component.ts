import { Component, OnInit } from '@angular/core';
import { UserdataService } from '../../../services/userDataService/userdata.service';
import { DatePipe, NgFor, NgIf } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { JwtDecodeService } from '../../../services/jwtDecodeService/jwtDecode.service';
import { OrganizerEventInterface } from '../../../interface/organizerInterface/organizer-event-interface';

@Component({
  selector: 'app-myevents',
  standalone: true,
  imports: [NgFor, NgIf, DatePipe],
  templateUrl: './myevents.component.html',
  styleUrl: './myevents.component.css',
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

  //getOrganizerEvents
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
        },
        (error) => console.error('Error fetching events: ', error)
      );
    } else {
      this.userdataservice.getOrganizerEvents().subscribe(
        (response: OrganizerEventInterface) => {
          this.events = response.allEvents;
        },
        (error) => console.error('Error fetching events: ', error)
      );
    }
  }
}
