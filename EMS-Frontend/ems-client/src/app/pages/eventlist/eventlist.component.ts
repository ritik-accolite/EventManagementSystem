import { NgFor, NgIf } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { UserdataService } from '../../userdata.service';

@Component({
  selector: 'app-eventlist',
  standalone: true,
  imports: [NgIf,NgFor],
  templateUrl: './eventlist.component.html',
  styleUrl: './eventlist.component.css'
})
export class EventlistComponent implements OnInit{
  title = 'ems-client';
  events: any[] = [];
  organizer: any[] = [];

  constructor(private http: HttpClient , private userdataservice: UserdataService) { }

  ngOnInit(): void {
    this.fetchEvents();
  }


  fetchEvents(): void {
    this.userdataservice.getEvents()
      .subscribe(
        (response : any ) => { // change any here !!
          this.events = response.allEvents; 
          console.log('Events fetched successfully:', this.events);
        },
        error => console.error('Error fetching events: ', error)
      );
  }
  }

