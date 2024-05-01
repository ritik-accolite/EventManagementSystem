import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UserdataService } from '../../userdata.service';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {

  username: string = '';
  password: string = '';

  constructor(private http: HttpClient,
    private userDataService: UserdataService,
    private router:Router
  ) {}

  onSubmit(): void {
   
    const credentials = {
      email: this.username,
      password: this.password
    };

   
    this.userDataService.loginUser(credentials)
      .subscribe(
        (response: any) => {
          this.router.navigate(['/Home']);
          console.log('Login successful:', response);
        },
        (error: any) => {
          console.error('Login failed:', error);
        }
      );
  }
}

