import { Component, OnInit } from '@angular/core';
import { UserdataService } from '../../userdata.service';
import { NgFor, DatePipe } from '@angular/common';
import { JwtDecodeService } from '../../services/jwtDecode.service';

@Component({
  selector: 'app-organizerstat',
  standalone: true,
  imports: [NgFor, DatePipe],
  templateUrl: './organizerstat.component.html',
  styleUrl: './organizerstat.component.scss'
})
export class OrganizerstatComponent implements OnInit { 
  events: any[] = [];
  trackEventTicket: any[] = [];
  previousEvents: any[] = [];
  upcomingEvents: any[] = [];
  totalEvents : number = 0;
  selectedCategory: string = 'All'; // Default category is 'all'
  totalRevenue: number = 0;
  totalAttendees: number = 0;
  
  constructor(private userdataservice: UserdataService, private jwtDecodeService : JwtDecodeService){}

  ngOnInit(): void {
    this.fetchEvents();
  }

  fetchEvents(): void {
    this.userdataservice.getOrganizerEvents()
      .subscribe(
        (response : any ) => {
          this.events = response;
          this.totalEvents = this.events.length;
          this.fetchTicketsTrack(this.jwtDecodeService.id);
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
