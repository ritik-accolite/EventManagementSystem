import { Component, OnInit } from '@angular/core';
import { UserdataService } from '../../../services/userDataService/userdata.service';
import { NgFor, DatePipe, DecimalPipe } from '@angular/common';
import { Router } from '@angular/router';
import { OrganizerEventInterface } from '../../../interface/organizerInterface/organizer-event-interface';
import { EventTicketInterface } from '../../../interface/organizerInterface/event-ticket-status';

@Component({
  selector: 'app-organizerstat',
  standalone: true,
  imports: [NgFor, DatePipe],
  templateUrl: './organizerstat.component.html',
  styleUrl: './organizerstat.component.scss',
})
export class OrganizerstatComponent implements OnInit {
  loginUserId: any;
  mergedEvent: any[] = [];
  events: any[] = [];
  trackEventTicket: EventTicketInterface[] = [];
  previousEvents: any[] = [];
  upcomingEvents: any[] = [];
  totalEvents: number = 0;
  selectedCategory: string = 'All';
  totalRevenue: number = 0;
  totalAttendees: number = 0;
  eventIdArray: number[] = [];
  totalRating: number = 0;
  totalReview: number = 0;
  reviews: any[] = [];

  constructor(
    private userdataservice: UserdataService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.fetchEvents();
  }

  fetchEvents(): void {
    this.userdataservice.getOrganizerEvents().subscribe(
      (response: OrganizerEventInterface) => {
        this.events = response.allEvents;
        this.totalEvents = this.events.length;
        this.loginUserId = localStorage.getItem('LoginUserId');
        this.fetchTicketsTrack(this.loginUserId);
      },
      (error) => console.error('Error fetching events: ', error)
    );
  }

  fetchTicketsTrack(organizerId: string): void {
    this.userdataservice.getOrganizerEventTicketDetails(organizerId).subscribe(
      (response: any) => {
        this.trackEventTicket = response.ticketStatus.events;
        this.mergeEventData();
      },
      (error) => console.error('Error fetching events: ', error)
    );
  }
  viewEvent(eventId: number) {
    this.userdataservice.eventId = eventId;
    this.router.navigate(['organizer-dash', 'app-viewevent']);
  }
  mergeEventData(): void {
    interface MergedEvent {
      eventId: number;
      eventName: string;
      eventDate: Date;
      chiefGuest: string;
      eventLocation: string;
      eventDescription: string;
      bannerImage: string,
      ticketPrice: number;
      totalTicketsSold: number;
      totalTicketsLeft: number;
    }

    if (this.events.length > 0 && this.trackEventTicket.length > 0) {
      const mergedEvents: MergedEvent[] = [];

      this.events.forEach((event) => {
        this.eventIdArray.push(event.eventId);
        const correspondingTicket = this.trackEventTicket.find(
          (ticket) => ticket.eventId === event.eventId
        );
        if (correspondingTicket) {
          // Merge ticket information into the event object
          const mergedEvent: MergedEvent = {
            eventId: event.eventId,
            eventName: event.eventName,
            eventDate: event.eventDate,
            chiefGuest: event.chiefGuest,
            eventLocation: event.eventLocation,
            eventDescription: event.eventDescription,
            bannerImage: event.bannerImage,
            ticketPrice: correspondingTicket.ticketPrice,
            totalTicketsSold: correspondingTicket.totalTicketsSold,
            totalTicketsLeft: correspondingTicket.totalTicketsLeft,
          };
          mergedEvents.push(mergedEvent);
        }
      });

      this.mergedEvent = mergedEvents;
      this.calculateTotalRevenue();
      this.calculateTotalAttendees();
      this.eventIdArray.forEach((eventId) => {
        this.userdataservice
          .getAllReviewsByEventId(eventId)
          .subscribe((data) => {
            this.reviews = data.allReviews;
            this.reviews.forEach((review) => {
              this.totalReview = this.totalReview + 1;
              this.totalRating =
                this.totalRating + parseInt(review['rating'], 10);
            });
          });
      });
    }
  }

  calculateTotalRevenue(): void {
    this.totalRevenue = this.mergedEvent.reduce((total, event) => {
      const ticketsSold = parseInt(String(event.totalTicketsSold), 10);
      const ticketPrice = event.ticketPrice;
      const eventRevenue = ticketsSold * ticketPrice;
      return total + eventRevenue;
    }, 0);
  }

  calculateTotalAttendees(): void {
    this.totalAttendees = this.mergedEvent.reduce((total, event) => {
      const ticketsSold = parseInt(String(event['totalTicketsSold']), 10);
      return total + ticketsSold;
    }, 0);
  }

  getReviewRatio(): string {
    const ratio =
      this.totalReview !== 0 ? this.totalRating / this.totalReview : 0;
    const decimalPipe = new DecimalPipe('en-US');
    return decimalPipe.transform(ratio, '1.1-1') || '0'; // Default to '0' if ratio is NaN
  }

  get filteredEvents(): any[] {
    if (this.selectedCategory === 'Upcoming') {
      return this.mergedEvent
        .filter((event) => new Date(event.eventDate) > new Date())
        .sort(
          (a, b) =>
            new Date(a.EventDate).getTime() - new Date(b.eventDate).getTime()
        );
    } else if (this.selectedCategory === 'Previous') {
      return this.mergedEvent
        .filter((event) => new Date(event.eventDate) < new Date())
        .sort(
          (a, b) =>
            new Date(b.EventDate).getTime() - new Date(a.eventDate).getTime()
        );
    } else {
      return this.mergedEvent;
    }
  }
}
