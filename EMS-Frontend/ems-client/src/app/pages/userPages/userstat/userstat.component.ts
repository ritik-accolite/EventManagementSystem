import { CommonModule, DatePipe , NgFor } from '@angular/common';
import { Component , HostListener, OnInit} from '@angular/core';
import { UserdataService } from '../../../services/userDataService/userdata.service';
import { JwtDecodeService } from '../../../services/jwtDecodeService/jwtDecode.service';
import { Router, RouterOutlet } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { EventbycategoryComponent } from "../eventbycategory/eventbycategory.component";
import { EventByCategoriesInterface } from '../../../interface/userInterface/event-by-categories-interface';
import { AllEventInterface } from '../../../interface/commonInterface/all-event-interface';
import { EventUserInterface } from '../../../interface/userInterface/event-user-interface';
import { EventCategoryInterface } from '../../../interface/userInterface/event-category-interface';
import { EventInterface } from '../../../interface/commonInterface/event-interface';

@Component({
    selector: 'app-userstat',
    standalone: true,
    templateUrl: './userstat.component.html',
    styleUrl: './userstat.component.css',
    imports: [NgFor, DatePipe, FormsModule, CommonModule, RouterOutlet, EventbycategoryComponent]
})
export class UserstatComponent implements OnInit {
  events: any[] = [];
  showDropdown1 = false;
  allEventCategory: any[] = [];
  showDropdown2 = false;
  categoryFilter: any[] = [];
  locationFilter: any[] = [];
  totalEvents : number = 0;
  selectedField : string = 'All';
  selectedCategory: string = ''; 
  selectedLocation: string = '';
  filteredEvents: any[] = [];
  searchKeyword: string = '';
  
  constructor(private userdataservice: UserdataService, private jwtDecodeService : JwtDecodeService ,
    private router: Router){}

  ngOnInit(): void {
    this.fetchEvents();
    // this.fetchEventCategories();
    // this.fetchEventLocations();
  }

  fetchEvents(): void {
    this.userdataservice.getEvents()
      .subscribe(
        (response : EventInterface ) => { // change any here !!
          console.log(response);
          this.events = response.allEvents; 
          this.filteredEvents = response.allEvents;
          console.log('Events fetched successfully:', this.events);
        },
        error => console.error('Error fetching events: ', error)
      );
      this.fetchEventCategories();
      this.fetchEventLocations();
  }

  // toggleDropdown() {
  //   this.showDropdown1 = !this.showDropdown1;
  //   this.showDropdown2 = !this.showDropdown2;
  // }

  toggleDropdown1() {
    this.showDropdown1 = true;
    //this.showDropdown2 = false;
}

 toggleDropdown2() {
    this.showDropdown2 = true;
    //this.showDropdown1 = false;
}

  fetchEventCategories() {
    this.userdataservice.getEventCategories().subscribe(
      (response: EventCategoryInterface) => {
        this.categoryFilter = response.allEventCategory;
        console.log(this.allEventCategory);
      },
      (error: any) => {
        console.error('Error fetching event categories:', error);
      }
    );
  }

  fetchEventLocations() {
    this.userdataservice.getEventLocations().subscribe(
      (response: EventCategoryInterface) => {
        this.locationFilter = response.allEventCategory;
      },
      (error: any) => {
        console.error('Error fetching event categories:', error);
      }
    );
  }

  applyFilter(selectedCategory: string) {
    this.selectedField = "Category";
    this.selectedCategory = selectedCategory;
    this.viewEvent();
    // console.log(selectedCategory);
    // this.router.navigate(['user-dash' , 'event-by-category']);
  }

  applyFilter1(selectedLocation: string) {
    this.selectedField = "Location";
    this.selectedLocation = selectedLocation;
    this.viewEvent();
    // console.log(selectedLocation);
    // this.router.navigate(['user-dash' , 'event-by-location']);
  }

  filterEvents() {
    if (this.searchKeyword.trim() === '') {
        this.filteredEvents = this.events;
    } else {
        this.filteredEvents = this.events.filter(event =>
            event.eventName.toLowerCase().includes(this.searchKeyword.toLowerCase())
        );
    }
}

  viewEvent() {
    if (this.selectedField === "Category") {
    // if (this.showDropdown1) {
      this.userdataservice.getEventsByCategory(this.selectedCategory).subscribe(
        (response: EventByCategoriesInterface) => {
          this.events = response.categoryEvents;
          console.log(response.categoryEvents);
        },
        (error: any) => {
          console.error('Error fetching events by category:', error);
        }
      );
    }

    else if (this.selectedField === "Location") {
      this.userdataservice.getEventsByLocation(this.selectedLocation).subscribe(
        (response: EventByCategoriesInterface) => {
          this.events = response.categoryEvents;
          console.log(response.categoryEvents);
        },
        (error: any) => {
          console.error('Error fetching events by category:', error);
        }
      );
    }

    else {
      this.fetchEvents();
    }
  }

  showDetail(eventId: number) {
    this.userdataservice.eventId = eventId;
    this.router.navigate(['user-dash','event-detail']);
  }


  // get filteredEvents(): EventUserInterface[] {
  // //   if (this.selectedField === 'Category') {
  //     return this.events.filter(event => new Date(event.eventDate) > new Date())
  // }
  //                       .sort((a, b) => new Date(a.eventDate).getTime() - new Date(b.eventDate).getTime());
  //   } else if (this.selectedField === 'Location') {
  //     return this.events.filter(event => new Date(event.eventDate) < new Date())
  //                       .sort((a, b) => new Date(b.eventDate).getTime() - new Date(a.eventDate).getTime());
  //   } else {
  //     return this.events;
  //   }
  // }

}
