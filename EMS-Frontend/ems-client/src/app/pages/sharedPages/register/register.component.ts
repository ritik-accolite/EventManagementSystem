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
      firstname: ['',[Validators.required,Validators.minLength(3),
        Validators.maxLength(60),
        Validators.pattern(/^[a-zA-Z]*$/),]],
      lastname: ['', [Validators.required,Validators.minLength(3),
        Validators.maxLength(60),
        Validators.pattern(/^[a-zA-Z]*$/),]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6),Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$/)
      ]],
      phonenumber: ['', [Validators.required,Validators.minLength(10),Validators.maxLength(10),Validators.pattern(/^[0-9]{10}$/)]],
      role: ['', [Validators.required]],
      confirmPassword: [
        '',
        [
          Validators.required,
          Validators.minLength(6),
          Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$/)
        ],
      ],
    },{
      validators: this.passwordMatchValidator
    });
  }
passwordMatchValidator(group: FormGroup) {
    // const password = group.get('password')?.value;
    // const confirmPassword = group.get('confirmPassword')?.value;
    // if (password === confirmPassword) {
    //   return null;
    // } else {
    //   return { mismatch: true };
    // }
    const password = group.get('password');
    const confirmPassword = group.get('confirmPassword');
      // console.log(password);
      // console.log(confirmPassword);
    // return password === confirmPassword ? null : { passwordNotMatch: true };
    if (password?.value !== confirmPassword?.value) {
      confirmPassword?.setErrors({ passwordNotMatch : true });
    } else {
      confirmPassword?.setErrors(null);
    }
  }
  onSubmit() {

    if (this.userDataForm.valid) {
      this.userDataForm.value.phonenumber = this.userDataForm.value.phonenumber.toString();
      this.userDataService.registerUser(this.userDataForm.value).subscribe(
        (response) => {
          if( response.status == 403){
            this.toaster.error(response.message);
            return;
          } else if (response.status==401){
            this.toaster.error(response.message);
            return;
          } else if(response.status == 200){
          this.router.navigate(['/login']);
          this.toaster.success(response.message);
          return;
        } else{
          this.toaster.success('Something went wrong!');
          return;
        }
        },
        (error) => {
          console.error('Error registering user:', error);
          this.toaster.error("Something went wrong!");
        }
      );
    } else {
      this.userDataForm.markAllAsTouched();
    }
  }
}
