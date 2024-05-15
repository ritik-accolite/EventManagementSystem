import { Component, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { NeweventComponent } from '../../organizerPages/newevent/newevent.component';
import { JwtDecodeService } from '../../../services/jwtDecodeService/jwtDecode.service';
import { CommonModule } from '@angular/common';
import { FormsModule} from '@angular/forms';
import { UserdataService } from '../../../services/userDataService/userdata.service';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [NeweventComponent, RouterLink, CommonModule , FormsModule],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css'
})
export class SidebarComponent {
  role : string = '';
  showDropdown1 = false;
  allEventCategory: string[] = [];
  selectedCategory: string = '';
  showDropdown2 = false;

  constructor(private router: Router,
              private jwtDecodeService : JwtDecodeService,
              private userdataservice :  UserdataService
  ) {}

  ngOnInit(): void{
    this.role = this.jwtDecodeService.role;
    if (this.role === null) {
      this.router.navigate(['/login']);
    }
    this.fetchEventCategories();
  }

  onTabClick(tabRoute: string) {
    if (tabRoute === 'user-profile') {
      this.router.navigate(['user-dash', 'user-profile']);
    } else if (tabRoute === 'myevents') {
      this.router.navigate(['user-dash','app-myevents']);
    } else if (tabRoute === 'new-event')  {
      this.router.navigate(['user-dash', 'new-event'])
    } else if (tabRoute === 'event-list') {
      this.router.navigate(['user-dash','event-list'])
    } else if (tabRoute === 'mybookings') {
      this.router.navigate(['user-dash','mybookings'])
    } else if (tabRoute === 'organizer-stat') {
      this.router.navigate(['user-dash','app-organizerstat'])
    } else if (tabRoute === 'organizer-profile') {
      this.router.navigate(['admin-dash','user-profile'])
    } else if (tabRoute === 'track-event') {
      this.router.navigate(['admin-dash','track-event'])
    } else if (tabRoute === 'track-organizer') {
      this.router.navigate(['admin-dash','track-organizer'])
    } else if (tabRoute === 'track-user') {
      this.router.navigate(['admin-dash','track-organizer'])
    } else if (tabRoute === 'issues') {
      this.router.navigate(['admin-dash','issues'])
    }
  }

  toggleDropdown() {
    this.showDropdown1 = !this.showDropdown1;
    this.showDropdown2 = !this.showDropdown2;
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


  applyFilter(selectedCategory: string) {
    this.userdataservice.selectedCategory = selectedCategory;
    console.log(selectedCategory);
    this.router.navigate(['user-dash' , 'event-by-category']);
  }

  applyFilter1(selectedLocation: string) {
    this.userdataservice.selectedCategory = selectedLocation;
    console.log(selectedLocation);
    this.router.navigate(['user-dash' , 'event-by-location']);
  }

}
