import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, Router, RouterOutlet } from '@angular/router';
import { UserdataService } from '../../userdata.service';
import { AngularFontAwesomeComponent } from 'angular-font-awesome';
import { FormsModule } from '@angular/forms';

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

  constructor(private userDataService: UserdataService, private router: Router) { }

  ngOnInit(): void {
    this.userDataService.loginEvent.subscribe((loggedIn: boolean) => {
      this.isLoggedIn = loggedIn; // Update isLoggedIn state upon login event
    });
  }

  logout(): void {
    // Call the logout method which returns an Observable
    this.userDataService.logout().subscribe(
      (response: any) => {
        console.log('Logout successful:', response.message); // Print the response data
        this.isLoggedIn = !this.isLoggedIn;
        this.router.navigate(['/login']);

      },
      (error: any) => {
        console.error('Logout failed:', error.message); // Handle error if any
      }
    );
  }
  // logout() {
  //   this.userDataService.logout().subscribe(() => {
  //     // On successful logout, navigate to the login page
  //     this.router.navigate(['/login']);
  //   });
  // }  searchQuery: string = '';



  search() {
    // Perform search action with the searchQuery value
    console.log('Search query:', this.searchQuery);
    // You can call a service method here to perform the actual search
  }

}
