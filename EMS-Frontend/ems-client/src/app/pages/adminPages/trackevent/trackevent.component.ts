import { Component } from '@angular/core';
import { UserdataService } from '../../../services/userDataService/userdata.service';
import { NgFor, NgIf } from '@angular/common';

@Component({
  selector: 'app-trackevent',
  standalone: true,
  imports: [NgFor , NgIf],
  templateUrl: './trackevent.component.html',
  styleUrl: './trackevent.component.css'
})
export class TrackeventComponent {
  events : any [] = [];
  constructor(private userdataservice : UserdataService) {}

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
