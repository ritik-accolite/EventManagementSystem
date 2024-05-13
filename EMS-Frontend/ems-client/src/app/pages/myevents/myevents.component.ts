import { Component, OnInit } from '@angular/core';
import { UserdataService } from '../../userdata.service';
import { NgFor } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-myevents',
  standalone: true,
  imports: [NgFor],
  templateUrl: './myevents.component.html',
  styleUrl: './myevents.component.css'
})
export class MyeventsComponent implements OnInit { 
  events: any[] = [];
  constructor(private userdataservice: UserdataService, private router: Router   ){}

  //getOrganizerEvents
  ngOnInit(): void {
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
    this.userdataservice.getOrganizerEvents()
      .subscribe(
        (response : any ) => { // change any here !!
          this.events = response;
          console.log('Events fetched successfully 123:', this.events);
        },
        error => console.error('Error fetching events: ', error)
      );
  }
}
