import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, Router, RouterOutlet } from '@angular/router';
import { UserdataService } from '../../../services/userDataService/userdata.service';
import { JwtDecodeService } from '../../../services/jwtDecodeService/jwtDecode.service';
import { JwtHelperService } from '@auth0/angular-jwt';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [RouterLink, CommonModule ],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent implements OnInit {
  role : any;
  isLoggedIn: boolean = false;
  searchQuery: string = '';
  dashboardLink: string = '/Home'; 

  constructor(private userDataService: UserdataService,
              private jwtDecodeService : JwtDecodeService,
              private router: Router,
              private jwtHelper: JwtHelperService) { }

  ngOnInit(): void {
      this.role = localStorage.getItem('Role');
      console.log('Role while loading: ', this.role);
      if (this.role === null) {
        this.router.navigate(['/login']);
        this.isLoggedIn = false;
      }
      else{
      this.isLoggedIn = true;
      }

      this.userDataService.loginEvent.subscribe((LoggedIn) =>{
        this.isLoggedIn =  LoggedIn;
      })
      console.log('login stat :', this.isLoggedIn);
      this.userDataService.roleEvent.subscribe((role) =>{
        this.setDashboardLink(role);
      })
  }
  loggedInFunc() : boolean{

    try{
    if (localStorage.getItem('jwt') && !this.jwtHelper.isTokenExpired(localStorage.getItem('jwt'))){
      return true;
    }
    return false;
    }
    catch{
    return false;
    }
  }

  logout(): void {
    // Call the logout method which returns an Observable
    this.userDataService.logout().subscribe(
      (response: any) => {
        localStorage.removeItem("jwt");
        localStorage.removeItem('Role');
        console.log('Logout successful:', response.message); // Print the response data
        this.isLoggedIn = false;
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
        this.dashboardLink = '/admin-dash/track-event';
        break;
      default:
        this.dashboardLink = '/Home';
        break;
    }
  }

  search() {
    console.log('Search query:', this.searchQuery);
  }

}
