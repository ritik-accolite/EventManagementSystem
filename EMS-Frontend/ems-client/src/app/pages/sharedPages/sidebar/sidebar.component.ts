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
  role : any;
  // showDropdown1 = false;
  // allEventCategory: string[] = [];
  // selectedCategory: string = '';
  // showDropdown2 = false;
  toggleBar = false;

  constructor(private router: Router,
              private jwtDecodeService : JwtDecodeService,
              private userdataservice :  UserdataService
  ) {}

  ngOnInit(): void{
    // this.role = this.jwtDecodeService.role;
    this.role = localStorage.getItem('Role');
    console.log('Role while loading: ', this.role);
    if (this.role === null) {
      this.router.navigate(['/login']);
    }
    // this.fetchEventCategories();
  }

  deactivateToggle():void{
    this.toggleBar = true;
    const doc = document.getElementById('navbarSupportedSideContent') as HTMLElement;
    doc.style.display = 'none';
  }

  activateToggle() : void{
    const doc = document.getElementById('navbarSupportedSideContent') as HTMLElement;
    if(this.toggleBar === true){
      doc.style.display = 'block';
      this.toggleBar = false;
    }

  }
  onTabClick(tabRoute: string) {
    if (tabRoute === 'organizer-profile') {
      this.router.navigate(['organizer-dash', 'user-profile']);
    } else if (tabRoute === 'user-profile') {
      this.router.navigate(['user-dash','user-profile']);
    } else if (tabRoute === 'myevents') {
      this.router.navigate(['organizer-dash','app-myevents']);
    } else if (tabRoute === 'new-event')  {
      this.router.navigate(['organizer-dash', 'new-event'])
    } else if (tabRoute === 'event-list') {
      this.router.navigate(['user-dash','event-list'])
    } else if (tabRoute === 'mybookings') {
      this.router.navigate(['user-dash','mybookings'])
    } else if (tabRoute === 'user-stat') {
      this.router.navigate(['user-dash',''])
    } else if (tabRoute === 'organizer-stat') {
      this.router.navigate(['organizer-dash','app-organizerstat'])
    } else if (tabRoute === 'organizer-profile') {
      this.router.navigate(['organizer-dash','user-profile'])
    } else if (tabRoute === 'track-event') {
      this.router.navigate(['admin-dash','track-event'])
    } else if (tabRoute === 'track-organizer') {
      this.router.navigate(['admin-dash','track-organizer'])
    } else if (tabRoute === 'track-user') {
      this.router.navigate(['admin-dash','track-organizer'])
    } else if (tabRoute === 'issues') {
      this.router.navigate(['admin-dash','issues'])
    }  else if (tabRoute === 'admin-profile') {
      this.router.navigate(['admin-dash','user-profile']);
    }else if (tabRoute === 'user-stat') {
      this.router.navigate(['user-dash','user-stat'])
    }
  }

  // toggleDropdown() {
  //   this.showDropdown1 = !this.showDropdown1;
  //   this.showDropdown2 = !this.showDropdown2;
  // }


  // fetchEventCategories() {
  //   this.userdataservice.getEventCategories().subscribe(
  //     (response: any) => {
  //       this.allEventCategory = response.allEventCategory;
  //     },
  //     (error: any) => {
  //       console.error('Error fetching event categories:', error);
  //     }
  //   );
  // }


  // applyFilter(selectedCategory: string) {
  //   this.userdataservice.selectedCategory = selectedCategory;
  //   console.log(selectedCategory);
  //   this.router.navigate(['user-dash' , 'event-by-category']);
  // }

  // applyFilter1(selectedLocation: string) {
  //   this.userdataservice.selectedCategory = selectedLocation;
  //   console.log(selectedLocation);
  //   this.router.navigate(['user-dash' , 'event-by-location']);
  // }

}
