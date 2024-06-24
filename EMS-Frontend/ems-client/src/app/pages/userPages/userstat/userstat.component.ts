import { CommonModule, DatePipe, NgFor } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { UserdataService } from '../../../services/userDataService/userdata.service';
import { JwtDecodeService } from '../../../services/jwtDecodeService/jwtDecode.service';
import { Router, RouterOutlet } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { EventbycategoryComponent } from '../eventbycategory/eventbycategory.component';
import { EventByCategoriesInterface } from '../../../interface/userInterface/event-by-categories-interface';
import { EventCategoryInterface } from '../../../interface/userInterface/event-category-interface';
import { EventInterface } from '../../../interface/commonInterface/event-interface';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-userstat',
  standalone: true,
  templateUrl: './userstat.component.html',
  styleUrls: ['./userstat.component.css'],
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
  filteredEvents: any[] = [];
  categoryFilter: any[] = [];
  locationFilter: any[] = [];
  searchKeyword: string = '';
  selectedCategory: string = '';
  selectedLocation: string = '';
  minPrice: number | null = null;
  maxPrice: number | null = null;

  constructor(
    private userdataservice: UserdataService,
    private jwtDecodeService: JwtDecodeService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.fetchEvents();
    this.fetchEventCategories();
    this.fetchEventLocations();
  }

  fetchEvents(): void {
    this.userdataservice.getEvents().subscribe(
      (response: EventInterface) => {
        this.events = response.allEvents;
        this.filteredEvents = response.allEvents;
      },
      (error) => console.error('Error fetching events: ', error)
    );
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
        console.error('Error fetching event locations:', error);
      }
    );
  }

  showFilterDialog() {
    const maxLength = 20; // Maximum characters to display
  
    const truncate = (str: string, max: number) =>
      str.length > max ? `${str.substring(0, max)}...` : str;
  
    Swal.fire({
      title: 'Apply Filters',
      html: `
        <div style="text-align: left;">
          <label for="category">Category:</label>
          <select id="category" class="swal2-input">
            <option class = "my-2"value="">All Categories</option>
            ${this.categoryFilter.map(category => `<option value="${category}">${truncate(category, maxLength)}</option>`).join('')}
          </select></br>
          <label for="location">Location:</label>
          <select id="location" class="swal2-input">
            <option value="">All Locations</option>
            ${this.locationFilter.map(location => `<option value="${location}">${truncate(location, maxLength)}</option>`).join('')}
          </select></br>
          <label for="minPrice">Min Price</label>
          <input type="number" id="minPrice" class="swal2-input" min="0"></br>
          <label for="maxPrice">Max Price</label>
          <input type="number" id="maxPrice" class="swal2-input">
        </div>
      `,
      showCancelButton: true,
      confirmButtonText: 'Apply Filters',
      preConfirm: () => {
        const category = (document.getElementById('category') as HTMLSelectElement).value;
        const location = (document.getElementById('location') as HTMLSelectElement).value;
        const minPrice = (document.getElementById('minPrice') as HTMLInputElement).value;
        const maxPrice = (document.getElementById('maxPrice') as HTMLInputElement).value;
  
        if (minPrice && parseFloat(minPrice) < 0) {
          Swal.showValidationMessage('Min Price cannot be less than 0');
          return false;
        }
  
        return {
          category: category,
          location: location,
          minPrice: minPrice ? parseFloat(minPrice) : null,
          maxPrice: maxPrice ? parseFloat(maxPrice) : null,
        };
      }
    }).then((result) => {
      if (result.isConfirmed) {
        this.selectedCategory = result.value.category;
        this.selectedLocation = result.value.location;
        this.minPrice = result.value.minPrice;
        this.maxPrice = result.value.maxPrice;
        this.applyFilters();
      }
    });
  }
  
  

  applyFilters() {
    this.filteredEvents = this.events.filter((event) => {
      const categoryMatch = this.selectedCategory ? event.eventCategory === this.selectedCategory : true;
      const locationMatch = this.selectedLocation ? event.eventLocation === this.selectedLocation : true;
      const minPriceMatch = this.minPrice !== null ? event.ticketPrice >= this.minPrice : true;
      const maxPriceMatch = this.maxPrice !== null ? event.ticketPrice <= this.maxPrice : true;
      return categoryMatch && locationMatch && minPriceMatch && maxPriceMatch;
    });
    this.applySearch();
  }

  applySearch() {
    const searchTerm = this.searchKeyword.trim().toLowerCase();
    if (searchTerm === '') {
      return; // Do nothing if search term is empty
    }

    this.filteredEvents = this.filteredEvents.filter((event) =>
      event.eventName.toLowerCase().includes(searchTerm) ||
      event.eventLocation.toLowerCase().includes(searchTerm) ||
      event.chiefGuest.toLowerCase().includes(searchTerm)
    );
  }

  onSearch() {
    this.applyFilters();
  }

  showDetail(eventId: number) {
    this.userdataservice.eventId = eventId;
    this.router.navigate(['user-dash', 'event-detail']);
  }

  applyFilter2() {
    this.selectedCategory = '';
    this.selectedLocation = '';
    this.searchKeyword = '';
    this.minPrice = null;
    this.maxPrice = null;
    this.fetchEvents();
  }
}
