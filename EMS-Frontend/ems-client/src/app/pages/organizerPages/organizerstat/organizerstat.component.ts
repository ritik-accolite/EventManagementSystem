import { Component, OnInit } from '@angular/core';
import { UserdataService } from '../../../services/userDataService/userdata.service';
import { NgFor, DatePipe } from '@angular/common';
import { JwtDecodeService } from '../../../services/jwtDecodeService/jwtDecode.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-organizerstat',
  standalone: true,
  imports: [NgFor, DatePipe],
  templateUrl: './organizerstat.component.html',
  styleUrl: './organizerstat.component.scss'
})
export class OrganizerstatComponent implements OnInit { 
  loginUserId : any;
  events: any[] = [];
  trackEventTicket: any[] = [];
  previousEvents: any[] = [];
  upcomingEvents: any[] = [];
  totalEvents : number = 0;
  selectedCategory: string = 'All'; // Default category is 'all'
  totalRevenue: number = 0;
  totalAttendees: number = 0;
  
  constructor(private userdataservice: UserdataService,
              private jwtDecodeService : JwtDecodeService,
              private router : Router){}

  ngOnInit(): void {
    this.fetchEvents();
  }

  fetchEvents(): void {
    this.userdataservice.getOrganizerEvents()
      .subscribe(
        (response : any ) => {
          this.events = response.allEvents;
          console.log(this.events);
          this.totalEvents = this.events.length;
          this.loginUserId = localStorage.getItem('LoginUserId');
          this.fetchTicketsTrack(this.loginUserId);
          this.mergeEventData();

        },
        error => console.error('Error fetching events: ', error)
      );
  }

  fetchTicketsTrack(organizerId : string): void {
    this.userdataservice.getOrganizerEventTicketDetails(organizerId)
      .subscribe(
        (response : any ) => {
          this.trackEventTicket = response.events;
          this.mergeEventData();
        },
        error => console.error('Error fetching events: ', error)
      );
  }
  viewEvent(eventId: number) {
    this.userdataservice.eventId = eventId;
    this.router.navigate(['organizer-dash','app-viewevent']);
  }
  mergeEventData(): void {
    // Check if both events and tickets are fetched
    if (this.events.length > 0 && this.trackEventTicket.length > 0) {
      // Merge event and ticket information based on eventId
      this.events.forEach(event => {
        const correspondingTicket = this.trackEventTicket.find(ticket => ticket.eventId === event.eventId);
        if (correspondingTicket) {
          // Merge ticket information into the event object
          Object.assign(event, correspondingTicket);
        }
      });
      this.calculateTotalRevenue();
      this.calculateTotalAttendees();
    }
  }
  calculateTotalRevenue(): void {
    this.totalRevenue = this.events.reduce((total, event) => {
      const ticketsSold = parseInt(String(event['totalTicketsSold']), 10);
      const ticketPrice = parseFloat(event['ticketPrice']);
      const eventRevenue =ticketsSold * ticketPrice;
      return total + eventRevenue;
    }, 0);
  }
  
  calculateTotalAttendees(): void {
    this.totalAttendees = this.events.reduce((total, event) => {
      const ticketsSold = parseInt(String(event['totalTicketsSold']), 10);
      return total + ticketsSold;
    }, 0);
  }
  
  
  get filteredEvents(): any[] {
    if (this.selectedCategory === 'Upcoming') {
      return this.events.filter(event => new Date(event.eventDate) > new Date())
                        .sort((a, b) => new Date(a.eventDate).getTime() - new Date(b.eventDate).getTime());
    } else if (this.selectedCategory === 'Previous') {
      return this.events.filter(event => new Date(event.eventDate) < new Date())
                        .sort((a, b) => new Date(b.eventDate).getTime() - new Date(a.eventDate).getTime());
    } else {
      return this.events;
    }
  }

}
