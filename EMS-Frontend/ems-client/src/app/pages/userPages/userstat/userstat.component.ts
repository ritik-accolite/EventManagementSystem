import { CommonModule, DatePipe , NgFor } from '@angular/common';
import { Component , OnInit} from '@angular/core';
import { UserdataService } from '../../../services/userDataService/userdata.service';
import { JwtDecodeService } from '../../../services/jwtDecodeService/jwtDecode.service';
import { Router, RouterOutlet } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { EventbycategoryComponent } from "../eventbycategory/eventbycategory.component";

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
  allEventCategory: string[] = [];
  showDropdown2 = false;
  categoryFilter: any[] = [];
  locationFilter: any[] = [];
  totalEvents : number = 0;
  selectedCategory: string = ''; 
  selectedLocation: string = '';
  
  constructor(private userdataservice: UserdataService, private jwtDecodeService : JwtDecodeService ,
    private router: Router){}

  ngOnInit(): void {
   // this.fetchEvents();
    this.fetchEventCategories();
    this.fetchEventLocations();
  }

  // fetchEvents(): void {
  //   this.userdataservice.getOrganizerEvents()
  //     .subscribe(
  //       (response : any ) => {
  //         this.events = response;
  //         this.totalEvents = this.events.length;

  //       },
  //       error => console.error('Error fetching events: ', error)
  //     );
  // }

  // toggleDropdown() {
  //   this.showDropdown1 = !this.showDropdown1;
  //   this.showDropdown2 = !this.showDropdown2;
  // }

  toggleDropdown1() {
    this.showDropdown1 = !this.showDropdown1;
    this.showDropdown2 = false;
}

 toggleDropdown2() {
    this.showDropdown2 = !this.showDropdown2;
    this.showDropdown1 = false;
}

  fetchEventCategories() {
    this.userdataservice.getEventCategories().subscribe(
      (response: any) => {
        this.allEventCategory = response.allEventCategory;
      },
      (error: any) => {
        console.error('Error fetching event categories:', error);
      }
    );
  }

  fetchEventLocations() {
    this.userdataservice.getEventCategories().subscribe(
      (response: any) => {
        this.allEventCategory = response.allEventCategory;
      },
      (error: any) => {
        console.error('Error fetching event categories:', error);
      }
    );
  }

  applyFilter(selectedCategory: string) {
    this.selectedCategory = selectedCategory;
    this.viewEvent();
    // console.log(selectedCategory);
    // this.router.navigate(['user-dash' , 'event-by-category']);
  }

  applyFilter1(selectedLocation: string) {
    this.userdataservice.selectedCategory = selectedLocation;
    // console.log(selectedLocation);
    // this.router.navigate(['user-dash' , 'event-by-location']);
  }

  viewEvent() {
    if (this.showDropdown1) {
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

    else {
      this.userdataservice.getEventsByLocation(this.selectedLocation).subscribe(
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

  // get filteredEvents(): any[] {
  //   if (this.selectedCategory === 'Category') {
  //     return this.events.filter(event => new Date(event.eventDate) > new Date())
  //                       .sort((a, b) => new Date(a.eventDate).getTime() - new Date(b.eventDate).getTime());
  //   } else if (this.selectedCategory === 'Previous') {
  //     return this.events.filter(event => new Date(event.eventDate) < new Date())
  //                       .sort((a, b) => new Date(b.eventDate).getTime() - new Date(a.eventDate).getTime());
  //   } else {
  //     return this.events;
  //   }
  // }

}
