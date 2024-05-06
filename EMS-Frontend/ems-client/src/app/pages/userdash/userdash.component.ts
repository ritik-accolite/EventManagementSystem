import { Component } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';
import { SidebarComponent } from '../sidebar/sidebar.component';
import { JwtHelperService } from '@auth0/angular-jwt';

@Component({
  selector: 'app-userdash',
  standalone: true,
  imports: [RouterLink , RouterOutlet , SidebarComponent],
  templateUrl: './userdash.component.html',
  styleUrl: './userdash.component.css'
})
export class UserdashComponent {

  constructor(private jwtHelper: JwtHelperService) { }
  isUserAuthenticated = (): boolean => {
    const token = localStorage.getItem("jwt");
    if (token && !this.jwtHelper.isTokenExpired(token)){
      return true;
    }
    return false;
  }
}
