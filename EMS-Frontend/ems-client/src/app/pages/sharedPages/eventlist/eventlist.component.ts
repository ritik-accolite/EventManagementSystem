import { NgFor, NgIf } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component , OnInit } from '@angular/core';
import { UserdataService } from '../../../services/userDataService/userdata.service';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { EventInterface } from '../../../interface/commonInterface/event-interface';

@Component({
  selector: 'app-eventlist',
  standalone: true,
  imports: [NgIf,NgFor, RouterLink ],
  templateUrl: './eventlist.component.html',
  styleUrl: './eventlist.component.css'
})
export class EventlistComponent implements OnInit{
  title = 'ems-client';
  events: any[] = [];
  organizer: any[] = [];

  constructor(private http: HttpClient , private userdataservice: UserdataService , private route: ActivatedRoute , private router: Router) { }

  ngOnInit(): void {
    this.fetchEvents(); 
  }

  fetchEvents(): void {
    this.userdataservice.getEvents()
      .subscribe(
        (response : EventInterface ) => { // change any here !!
          console.log(response);
          this.events = response.allEvents; 
          console.log('Events fetched successfully:', this.events);
        },
        error => console.error('Error fetching events: ', error)
      );
  }

  bookEvent(eventId: number, organizerId: string , ticketPrice: number): void {
    this.userdataservice.eventId = eventId;
    this.userdataservice.organizerId = organizerId;
    this.userdataservice.ticketPrice = ticketPrice;
    this.router.navigate(['event-bookings']);
  }
  

  }

