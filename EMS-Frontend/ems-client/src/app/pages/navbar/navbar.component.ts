import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, Router } from '@angular/router';
import { UserdataService } from '../../userdata.service';
@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [RouterLink, CommonModule],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent implements OnInit {
  isLoggedIn: boolean = false;

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
  // }
}
