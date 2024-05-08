import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UserdataService } from '../../userdata.service';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { JwtDecodeService } from '../../services/jwtDecode.service';

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
        },
        (error: any) => {
          console.error('Login failed:', error);
        }
      );
  }
}

