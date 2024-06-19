import { NgIf } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { UserdataService } from '../../../services/userDataService/userdata.service';
import { Router } from '@angular/router';
import { response } from 'express';
import { error } from 'console';
import { ForgetPasswordInterface } from '../../../interface/commonInterface/forget-password-interface';
import { ResponseInterface } from '../../../interface/commonInterface/response-interface';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-forgotpassword',
  standalone: true,
  imports: [FormsModule, NgIf],
  templateUrl: './forgotpassword.component.html',
  styleUrl: './forgotpassword.component.css',
})
export class ForgotpasswordComponent {
  showPasswordForm: boolean = false;
  email: string = '';
  emailToken: string = '';
  newPassword: string = '';
  confirmPassword: string = '';
  toaster=inject(ToastrService);

  constructor(
    private userDataService: UserdataService,
    private router: Router
  ) {}

  submitEmailForm() {
    //this.showPasswordForm = true;
    this.userDataService.generateForgotEmailToken(this.email).subscribe(
      (response: ResponseInterface) => {  
        if(response.status==200){
          this.toaster.info("Reset Token successfully Sent");
          console.log('Reset Token successfully Sent:', response);
          this.showPasswordForm = true;
        }
        else if(response.status==404){
          this.toaster.error("Email doesn't exist !");
        }
        else if(response.status==500){
          this.toaster.error("Failed to send reset password token !");
        }
      },
      (error: any) => {
        console.error('Reset Token not sent:', error);
      }
    );
  }

  submitPasswordForm() {
    const resetForm = {
      email: this.email,
      emailToken: this.emailToken,
      newPassword: this.newPassword,
      confirmPassword: this.confirmPassword,
    };

    this.userDataService.resetPassword(resetForm)
      .subscribe(
      (response : ResponseInterface) => {
        if(response.status==200){
        this.toaster.success("Successfully updated password");
        console.log('Password Successfully Reseted', response);
        }
        else if(response.status==404){
          this.toaster.error("Email doesn't exist !");
        }
        else if(response.status==400){
          this.toaster.error("Invalid reset link !");
        }
      },
      (error: any) => {
        this.toaster.error("Error updating Password");
        console.error('Password not sent: ', error);
      }
    );
  }
}
