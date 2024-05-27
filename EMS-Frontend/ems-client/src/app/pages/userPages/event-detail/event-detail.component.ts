import { Component, OnInit } from '@angular/core';
import { UserdataService } from '../../../services/userDataService/userdata.service';
import { ActivatedRoute , Router, RouterLink } from '@angular/router';
import { EventDetailInterface } from '../../../interface/userInterface/event-detail-interface';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-event-detail',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './event-detail.component.html',
  styleUrl: './event-detail.component.css'
})
export class EventDetailComponent implements OnInit {
  eventId: number = 0;
  eventDetails!: EventDetailInterface;

  constructor(private http: HttpClient , private userdataService : UserdataService , private router : Router ) {}

  ngOnInit(): void {
    this.eventId = this.userdataService.eventId;
    if (this.eventId) {
      this.userdataService.getEventById(this.eventId).subscribe(
        (eventDetails: EventDetailInterface) => {
          this.eventDetails = eventDetails;
          console.log(eventDetails.eventName);
        },
        (error) => {
          console.error("Error fetching event details:", error);
        }
      );
    }
  }

bookEvent(eventId: number, organizerId: string , ticketPrice: number): void {
  this.userdataService.eventId = eventId;
  this.userdataService.organizerId = organizerId;
  this.userdataService.ticketPrice = ticketPrice;
  this.router.navigate(['event-bookings']);
}

}
