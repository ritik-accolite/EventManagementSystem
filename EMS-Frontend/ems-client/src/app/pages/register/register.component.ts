import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserdataService } from '../../userdata.service';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule,
    HttpClientModule, RouterLink, CommonModule
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})

export class RegisterComponent implements OnInit {
  userDataForm!: FormGroup;
form: any;

  constructor(
    private fb: FormBuilder,
    private userDataService: UserdataService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.userDataForm = this.fb.group({
      firstname: ['', Validators.required],
      lastname: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6), Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*])(?=.*[0-9]).{6,}$/)]],
      phonenumber: ['', Validators.required, Validators.minLength(10), Validators.pattern('^[0-9]*$'), Validators.maxLength(10)],
      role: ['', Validators.required]
    });
  }
  onSubmit() {
    if (this.userDataForm && this.userDataForm.valid) {
      this.userDataService.registerUser(this.userDataForm.value).subscribe(
        response => {
          this.router.navigate(['/login']);
          console.log('User registered successfully:', response);
          // this.userDataForm.reset();
        },
        error => {
          console.error('Error registering user:', error);
        }
      );
    } else {
      this.userDataForm.markAllAsTouched();
    }
  }
}