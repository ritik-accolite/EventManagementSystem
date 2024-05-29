import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserdataService } from '../../../services/userDataService/userdata.service';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule, HttpClientModule, RouterLink, CommonModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
})
export class RegisterComponent implements OnInit {
  userDataForm!: FormGroup;
  toaster=inject(ToastrService);
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
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: [
        '',
        [
          Validators.required,
          Validators.minLength(6),
          this.passwordMatchValidator,
        ],
      ],
      phonenumber: ['', Validators.required, Validators.minLength(9), Validators.maxLength(10)],
      role: ['', Validators.required],
    });
  }
  passwordMatchValidator(group: FormGroup) {
    const password = group.get('password')?.value;
    const confirmPassword = group.get('confirmPassword')?.value;
    if (password === confirmPassword) {
      return null;
    } else {
      return { mismatch: true };
    }
  }
  onSubmit() {
    if (this.userDataForm.valid) {
      console.log('Inside');
      this.userDataService.registerUser(this.userDataForm.value).subscribe(
        (response) => {
          this.router.navigate(['/login']);
          this.toaster.success("Registered Successfully");
          console.log('User registered successfully:', response);
        },
        (error) => {
          console.error('Error registering user:', error);
        }
      );
    } else {
      this.userDataForm.markAllAsTouched();
    }
  }
}
