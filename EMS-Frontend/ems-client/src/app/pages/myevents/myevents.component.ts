import { Component } from '@angular/core';
import { UserdataService } from '../../userdata.service';

@Component({
  selector: 'app-myevents',
  standalone: true,
  imports: [],
  templateUrl: './myevents.component.html',
  styleUrl: './myevents.component.css'
})
export class MyeventsComponent { // implements OnInit{
  // events: any[] =[];
  // filteredEvents: any[] = [];
  // organizers: any[] = [];
  // selectedOrganizerId: string = '';

  // constructor(private userdataservice: UserdataService) {}

  // ngOnInit(): void {
  //     this.fetchEvents();
  //     this.fetchOrganizers();
  // }

  // fetchEvents() {
  //   this.userdataservice.getEvents().subscribe((events: any[]) => {
  //     this.events = events;
  //     this.filteredEvents();
  //   });
  // }

  // fetchOrganizers() {
  //   this.userdataservice.getOrganizers().subscribe((organizers: any[]) => {
  //     this.organizers = organizers;
  //   });
  // }

  // fetchEvents() {
  //   if (this.selectedOrganizerId) {
  //     this.filteredEvents = this.filter(events => event.organizerId === this.selectedOrganizerId);
  //   } else {
  //     this.filteredEvents = this.events;
  //   }
  // }
}
