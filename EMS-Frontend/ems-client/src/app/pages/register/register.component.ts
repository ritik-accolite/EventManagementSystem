import { Component, OnInit } from '@angular/core';
import { UserdataService } from '../../userdata.service';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule,
    HttpClientModule
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent implements OnInit {
  userDataForm!: FormGroup;
  constructor(
    private fb: FormBuilder,
    private userDataService: UserdataService
  ) {}
  ngOnInit(): void {
    this.userDataForm = this.fb.group({
      firstname: ['', Validators.required],
      lastname: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      phonenumber: ['', Validators.required],
      role: ['', Validators.required]
    });
  }
  onSubmit() {
    if (this.userDataForm.valid) {
      this.userDataService.registerUser(this.userDataForm.value).subscribe(
        response => {
          console.log('User registered successfully:', response);
          // Optionally, reset the form after successful registration
          this.userDataForm.reset();
        },
        error => {
          console.error('Error registering user:', error);
        }
      );
    } else {
      // Mark form controls as touched to display validation errors
      this.userDataForm.markAllAsTouched();
    }
  }
}