import { Component, OnInit } from '@angular/core';
import { UserdataService } from '../../../services/userDataService/userdata.service';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { DatePipe } from '@angular/common';
@Component({
  selector: 'app-event-detail',
  standalone: true,
  imports: [RouterLink, DatePipe],
  templateUrl: './event-detail.component.html',
  styleUrl: './event-detail.component.css',
})
export class EventDetailComponent implements OnInit {
  eventId: number = 0;
  eventDetails: any;
  constructor(
    private http: HttpClient,
    private userdataService: UserdataService,
    private router: Router
  ) {}
  ngOnInit(): void {
    this.eventId = this.userdataService.eventId;
    console.log('Event id', this.eventId);
    if (this.eventId) {
      this.userdataService.getEventById(this.eventId).subscribe(
        (eventDetails: any) => {
          console.log('event response', eventDetails.getEventById);
          this.eventDetails = eventDetails.getEventById;
        },
        (error) => {
          console.error('Error fetching event details:', error);
        }
      );
    }
  }
  bookEvent(eventId: number, organizerId: string, ticketPrice: number): void {
    this.userdataService.eventId = eventId;
    this.userdataService.organizerId = organizerId;
    this.userdataService.ticketPrice = ticketPrice;
    this.router.navigate(['user-dash/event-list', 'event-bookings']);
  }
}