import { Component } from '@angular/core';
import { UserdataService } from '../../../services/userDataService/userdata.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-eventbylocation',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './eventbylocation.component.html',
  styleUrl: './eventbylocation.component.css',
})
export class EventbylocationComponent {
  selectedLocation = this.userdataservice.selectedLocation;

  events: any[] = [];

  constructor(private userdataservice: UserdataService) {}

  ngOnInit() {
    this.userdataservice.getEventsByLocation(this.selectedLocation).subscribe(
      (response: any) => {
        this.events = response.categoryEvents;
      },
      (error: any) => {
        console.error('Error fetching events by category:', error);
      }
    );
  }
}
