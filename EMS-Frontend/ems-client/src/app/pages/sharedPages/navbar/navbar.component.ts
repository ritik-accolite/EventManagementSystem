import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, Router, RouterOutlet } from '@angular/router';
import { UserdataService } from '../../../services/userDataService/userdata.service';
import { FormsModule } from '@angular/forms';
import { JwtDecodeService } from '../../../services/jwtDecodeService/jwtDecode.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [RouterLink, CommonModule ],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent implements OnInit {
  isLoggedIn: boolean = false;
  searchQuery: string = '';
  dashboardLink: string = '/user-dash'; 

  constructor(private userDataService: UserdataService,
              private jwtDecodeService : JwtDecodeService,
              private router: Router) { }

  ngOnInit(): void {
    this.userDataService.loginEvent.subscribe((loggedIn: boolean) => {
      this.isLoggedIn = loggedIn; // Update isLoggedIn state upon login event
      const role = this.jwtDecodeService.role;
      this.setDashboardLink(role);
    });
  }

  logout(): void {
    // Call the logout method which returns an Observable
    this.userDataService.logout().subscribe(
      (response: any) => {
        localStorage.removeItem("jwt");
        console.log('Logout successful:', response.message); // Print the response data
        this.isLoggedIn = !this.isLoggedIn;
        this.router.navigate(['/login']);

      },
      (error: any) => {
        console.error('Logout failed:', error.message); // Handle error if any
      }
    );
  }

  setDashboardLink(role: string): void {
    switch (role) {
      case 'Organizer':
        this.dashboardLink = '/organizer-dash';
        break;
      case 'User':
        this.dashboardLink = '/user-dash';
        break;
      case 'Admin':
        this.dashboardLink = '/admin-dash';
        break;
      default:
        this.dashboardLink = '/user-dash'; // Default to user dashboard
        break;
    }
  }

  search() {
    // Perform search action with the searchQuery value
    console.log('Search query:', this.searchQuery);
    // You can call a service method here to perform the actual search
  }

}
