import { Component, OnInit } from '@angular/core';
import { UserdataService } from '../../../services/userDataService/userdata.service';
import { NgFor, NgIf } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { JwtDecodeService } from '../../../services/jwtDecodeService/jwtDecode.service';
import { OrganizerEventInterface } from '../../../interface/organizerInterface/organizer-event-interface';

@Component({
  selector: 'app-myevents',
  standalone: true,
  imports: [NgFor, NgIf],
  templateUrl: './myevents.component.html',
  styleUrl: './myevents.component.css'
})
export class MyeventsComponent implements OnInit { 
  events: any[] = [];
  role : string = '';
  organizerId : string = '';
  constructor(private userdataservice: UserdataService,
              private router: Router,
              private jwtDecodeService : JwtDecodeService,
              private route : ActivatedRoute   ){}

  //getOrganizerEvents
  ngOnInit(): void {
    this.role = this.jwtDecodeService.role;
    this.route.params.subscribe(params => {
    this.organizerId = this.jwtDecodeService.organizerId;
      // Now you can use this.organizerId as needed
      console.log('Organizer ID:', this.organizerId);
    });
    this.fetchEvents();
  }

  editEvent(eventId: number) {
    this.userdataservice.eventId = eventId;
    this.router.navigate(['user-dash','app-editevent']);
  }
  viewEvent(eventId: number) {
    this.userdataservice.eventId = eventId;
    this.router.navigate(['user-dash','app-viewevent']);
  }
  
  fetchEvents(): void {
    if(this.organizerId!=''){

      this.userdataservice.getOrganizerEventsById(this.organizerId)
      .subscribe(
        (response : OrganizerEventInterface ) => { // change any here !!
          this.events = response.allEvents;
          console.log('Events fetched successfully 123:', this.events);
        },
        error => console.error('Error fetching events: ', error)
      );
    }
    else 
    {
    this.userdataservice.getOrganizerEvents()
      .subscribe(
        (response : OrganizerEventInterface ) => { // change any here !!
          this.events = response.allEvents;
          console.log('Events fetched successfully 123:', this.events);
        },
        error => console.error('Error fetching events: ', error)
      );
    }  
  }
}
