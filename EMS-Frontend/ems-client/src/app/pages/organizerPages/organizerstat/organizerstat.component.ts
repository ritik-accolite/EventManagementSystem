import { Component, OnInit } from '@angular/core';
import { UserdataService } from '../../../services/userDataService/userdata.service';
import { NgFor, DatePipe } from '@angular/common';
import { JwtDecodeService } from '../../../services/jwtDecodeService/jwtDecode.service';
import { Router } from '@angular/router';
import { OrganizerEventInterface } from '../../../interface/organizerInterface/organizer-event-interface';
import { AllEventInterface } from '../../../interface/commonInterface/all-event-interface';
import { EventTicketInterface, EventTicketStatus } from '../../../interface/organizerInterface/event-ticket-status';
import { MergedEventInterface } from '../../../interface/organizerInterface/merged-event-interface';

@Component({
  selector: 'app-organizerstat',
  standalone: true,
  imports: [NgFor, DatePipe],
  templateUrl: './organizerstat.component.html',
  styleUrl: './organizerstat.component.scss'
})
export class OrganizerstatComponent implements OnInit { 
  loginUserId : any;
  mergedEvent: MergedEventInterface [] =[];
  events: AllEventInterface [] = [];
  trackEventTicket: EventTicketInterface[] = [];
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
        (response : OrganizerEventInterface ) => {
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
        (response : EventTicketStatus ) => {
          console.log(response);
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
      // Create a new array to store the merged data
      const mergedEvents: MergedEventInterface[] = [];
    
      // Merge event and ticket information based on eventId
      this.events.forEach(event => {
        const correspondingTicket = this.trackEventTicket.find(ticket => ticket.eventId === event.EventId);
        if (correspondingTicket) {
          // Merge ticket information into the event object
          const mergedEvent: MergedEventInterface = Object.assign({}, event, correspondingTicket);
          mergedEvents.push(mergedEvent);
        }
      });
  
      // Assign the merged events array to the class property
      this.mergedEvent = mergedEvents;
    
      // Calculate total revenue and total attendees for the merged events
      this.calculateTotalRevenue();
      this.calculateTotalAttendees();
    }
  }
  
  
  calculateTotalRevenue(): void {
    this.totalRevenue = this.mergedEvent.reduce((total, event) => {
      const ticketsSold = parseInt(String(event.totalTicketsSold), 10);
      const ticketPrice = event.ticketPrice; // Assuming ticketPrice is already a number
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
  
  
  get filteredEvents(): any[] {
    if (this.selectedCategory === 'Upcoming') {
      return this.events.filter(event => new Date(event.EventDate) > new Date())
                        .sort((a, b) => new Date(a.EventDate).getTime() - new Date(b.EventDate).getTime());
    } else if (this.selectedCategory === 'Previous') {
      return this.events.filter(event => new Date(event.EventDate) < new Date())
                        .sort((a, b) => new Date(b.EventDate).getTime() - new Date(a.EventDate).getTime());
    } else {
      return this.events;
    }
  }

}
