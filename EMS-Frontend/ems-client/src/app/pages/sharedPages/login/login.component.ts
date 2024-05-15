import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UserdataService } from '../../../services/userDataService/userdata.service';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { JwtDecodeService } from '../../../services/jwtDecodeService/jwtDecode.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [RouterLink, FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {

  username: string = '';
  password: string = '';
  invalidLogin: boolean = true;
  constructor(private http: HttpClient,
    private userDataService: UserdataService,
    private router:Router,
    private jwtDecodeService : JwtDecodeService
  ) {}

  onSubmit(): void {
   
    const credentials = {
      email: this.username,
      password: this.password
    };

   
    this.userDataService.loginUser(credentials)
      .subscribe(
        (response: any) => {
          const token = response.token;
          localStorage.setItem("jwt", token); 
          const decodedToken = this.jwtDecodeService.decodeToken(token);
          console.log('Decoded Token:', decodedToken);
          this.invalidLogin = false; 
          this.userDataService.loginEvent.emit(true);
          this.router.navigate(['/user-dash']);
          console.log('Login successful:', response);
          const role = this.jwtDecodeService.role;    
          // Redirect user based on role
          this.redirectUser(role);
        },
        (error: any) => {
          console.error('Login failed:', error);
        }
      );
  }
  
  redirectUser(role: string): void {
    switch (role) {
      case 'Organizer':
        this.router.navigate(['/user-dash']); // rename user-dash to org-dash
        break;
      case 'User':
        this.router.navigate(['/user-dash']); // new dash for user
        break;
      case 'Admin':
        this.router.navigate(['/admin-dash']);
        break;
      default:
        // Handle other roles or invalid roles here
        break;
    }
  }
}
