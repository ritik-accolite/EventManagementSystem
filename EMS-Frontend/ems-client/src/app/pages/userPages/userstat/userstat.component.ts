import { CommonModule, DatePipe, NgFor } from '@angular/common';
import { Component, HostListener, OnInit } from '@angular/core';
import { UserdataService } from '../../../services/userDataService/userdata.service';
import { JwtDecodeService } from '../../../services/jwtDecodeService/jwtDecode.service';
import { Router, RouterOutlet } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { EventbycategoryComponent } from '../eventbycategory/eventbycategory.component';
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
  imports: [
    NgFor,
    DatePipe,
    FormsModule,
    CommonModule,
    RouterOutlet,
    EventbycategoryComponent,
  ],
})
export class UserstatComponent implements OnInit {
  events: any[] = [];
  showDropdown1 = false;
  allEventCategory: any[] = [];
  showDropdown2 = false;
  categoryFilter: any[] = [];
  locationFilter: any[] = [];
  totalEvents: number = 0;
  selectedField: string = 'All';
  selectedCategory: string = '';
  selectedLocation: string = '';
  filteredEvents: any[] = [];
  searchKeyword: string = '';

  constructor(
    private userdataservice: UserdataService,
    private jwtDecodeService: JwtDecodeService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.fetchEvents();
  }

  fetchEvents(): void {
    this.userdataservice.getEvents().subscribe(
      (response: EventInterface) => {
        this.events = response.allEvents;
        this.filteredEvents = response.allEvents;
      },
      (error) => console.error('Error fetching events: ', error)
    );
    this.fetchEventCategories();
    this.fetchEventLocations();
  }
  toggleDropdown1() {
    this.showDropdown1 = !this.showDropdown1;
  }

  toggleDropdown2() {
    this.showDropdown2 = !this.showDropdown2;
  }
  hideDropdown(): void {
    this.showDropdown1 = false;
    this.showDropdown2 = false;
  }
  fetchEventCategories() {
    this.userdataservice.getEventCategories().subscribe(
      (response: EventCategoryInterface) => {
        this.categoryFilter = response.allEventCategory;
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
    this.selectedField = 'Category';
    this.selectedCategory = selectedCategory;
    this.showDropdown1 = false;
    this.viewEvent();
  }

  applyFilter1(selectedLocation: string) {
    this.selectedField = 'Location';
    this.showDropdown2 = false;
    this.selectedLocation = selectedLocation;
    this.viewEvent();
  }
  applyFilter2() {
    this.userdataservice.getEvents().subscribe(
      (response: EventInterface) => {
        this.events = response.allEvents;
        this.filteredEvents = response.allEvents;
      },
      (error) => console.error('Error fetching events: ', error)
    );
  }

  filterEvents() {
    if (this.searchKeyword.trim() === '') {
      this.filteredEvents = this.events;
    } else {
      this.filteredEvents = this.events.filter((event) =>
        event.eventName.toLowerCase().includes(this.searchKeyword.toLowerCase())
      );
    }
  }

  viewEvent() {
    // if (this.selectedField === 'Category') {
    //   this.userdataservice.getEventsByCategory(this.selectedCategory).subscribe(
    //     (response: EventByCategoriesInterface) => {
    //       this.events = response.categoryEvents;
    //     },
    //     (error: any) => {
    //       console.error('Error fetching events by category:', error);
    //     }
    //   );
    // } else if (this.selectedField === 'Location') {
    //   this.userdataservice.getEventsByLocation(this.selectedLocation).subscribe(
    //     (response: EventByCategoriesInterface) => {
    //       this.events = response.categoryEvents;
    //     },
    //     (error: any) => {
    //       console.error('Error fetching events by category:', error);
    //     }
    //   );
    // }

    if (this.selectedField === 'Category' && this.selectedCategory !== '') {
      this.userdataservice.getEventsByCategory(this.selectedCategory).subscribe(
        (response: EventByCategoriesInterface) => {
          this.events = response.categoryEvents;
          this.applyLocationFilter(); 
        },
        (error: any) => {
          console.error('Error fetching events by category:', error);
        }
      );
    } else if (this.selectedField === 'Location' && this.selectedLocation !== '') {
      this.userdataservice.getEventsByLocation(this.selectedLocation).subscribe(
        (response: EventByCategoriesInterface) => {
          this.events = response.categoryEvents;
          this.applyCategoryFilter(); 
        },
        (error: any) => {
          console.error('Error fetching events by location:', error);
        }
      );
    } else {
      this.fetchEvents();
    }
  }

  applyCategoryFilter() {
    if (this.selectedCategory !== '') {
      this.filteredEvents = this.events.filter(event =>
        event.category === this.selectedCategory
      );
    } else {
      this.filteredEvents = this.events;
    }
  }
  
  applyLocationFilter() {
    if (this.selectedLocation !== '') {
      this.filteredEvents = this.events.filter(event =>
        event.location === this.selectedLocation
      );
    } else {
      this.filteredEvents = this.events;
    }
  }

  showDetail(eventId: number) {
    this.userdataservice.eventId = eventId;
    this.router.navigate(['user-dash', 'event-detail']);
  }
}
