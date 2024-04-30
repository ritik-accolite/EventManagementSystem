import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UserdataService } from '../../userdata.service';
import { FormsModule } from '@angular/forms';
@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {

  // Properties to bind to form inputs
  username: string = '';
  password: string = '';

  constructor(private http: HttpClient,
    private userDataService: UserdataService
  ) {}

  onSubmit(): void {
    // Create an object with username and password
    const credentials = {
      email: this.username,
      password: this.password
    };

    // Send a POST request to the login endpoint
    this.userDataService.loginUser(credentials)
      .subscribe(
        (response: any) => {
          // Handle successful login response
          console.log('Login successful:', response);
          // Optionally, redirect the user or perform other actions
        },
        (error: any) => {
          // Handle login error
          console.error('Login failed:', error);
          // Optionally, display an error message to the user
        }
      );
  }
}

