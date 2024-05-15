import { Component } from '@angular/core';
import { UserdataService } from '../../../services/userDataService/userdata.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-eventbycategory',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './eventbycategory.component.html',
  styleUrl: './eventbycategory.component.css'
})
export class EventbycategoryComponent {
  selectedCategory = this.userdataservice.selectedCategory;

  events: any[] = [];

  constructor(private userdataservice: UserdataService) { }

  ngOnInit(){
    this.userdataservice.getEventsByCategory(this.selectedCategory).subscribe(
      (response: any) => {
        this.events = response.categoryEvents;
        console.log(response.categoryEvents);
      },
      (error: any) => {
        console.error('Error fetching events by category:', error);
      }
    );
  }

}
