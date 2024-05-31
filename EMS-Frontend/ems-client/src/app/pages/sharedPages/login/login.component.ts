import { Component, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UserdataService } from '../../../services/userDataService/userdata.service';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { JwtDecodeService } from '../../../services/jwtDecodeService/jwtDecode.service';
import { NgIf } from '@angular/common';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [RouterLink, FormsModule, NgIf, ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  // isLoading: boolean = false;
  loginDataForm!: FormGroup;
  toaster=inject(ToastrService);
  username: string = '';
  password: string = '';
  invalidLogin: boolean = true;
  constructor(private http: HttpClient,
    private userDataService: UserdataService,
    private router:Router,
    private jwtDecodeService : JwtDecodeService,
    private fb : FormBuilder
  ) {}

  ngOnInit(): void {
    this.loginDataForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  // handleLoading() : void {
  //   console.log("Logging")
  //   this.isLoading = !this.isLoading
  // }

  onSubmit(): void {
  
    this.userDataService.loginUser(this.loginDataForm.value)
      .subscribe(
        (response: any) => {
          if(response.status == 403){
            this.toaster.error("You have Been Blocked by Admin");
            this.invalidLogin = true; 
            return;
          } else if (response.status == 400){
            this.toaster.error("Please verify the token sent on mail.");
            this.invalidLogin = true; 
            return;
          } else if (response.status == 401){
            this.toaster.error("Please enter valid credentials.");
            this.invalidLogin = true;
          }
          this.toaster.success("Successfully Logged In");
          const token = response.token;
          localStorage.setItem("jwt", token); 
          const decodedToken = this.jwtDecodeService.decodeToken(token);
          this.invalidLogin = false; 
          this.userDataService.loginEvent.emit(true);
          this.router.navigate(['/user-dash']);
          const role = this.jwtDecodeService.role;
          this.redirectUser(role);
          this.userDataService.roleEvent.emit(role);
        },
        (error: any) => {
          console.error('Login failed:', error);
        }
      );
  }
  
  redirectUser(role: string): void {
    switch (role) {
      case 'Organizer':
        this.router.navigate(['/organizer-dash']);
        break;
      case 'User':
        this.router.navigate(['/user-dash']);
        break;
      case 'Admin':
        this.router.navigate(['/admin-dash']);
        break;
      default:
        break;
    }
  }
}

