import { NgIf } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { UserdataService } from '../../../userdata.service';
import { Router } from '@angular/router';
import { response } from 'express';
import { error } from 'console';

@Component({
  selector: 'app-forgotpassword',
  standalone: true,
  imports: [FormsModule, NgIf],
  templateUrl: './forgotpassword.component.html',
  styleUrl: './forgotpassword.component.css'
})
export class ForgotpasswordComponent {
  showPasswordForm: boolean = false;
  email: string = '';
  emailToken: string = '';
  newPassword: string = '';
  confirmPassword: string = '';

  constructor(private userDataService : UserdataService,
              private router : Router
   ) {}

  submitEmailForm() {
    // Submit email form logic here
    // For demo purposes, just toggle the visibility of the forms
    this.showPasswordForm = true;
    this.userDataService.generateForgotEmailToken(this.email)
      .subscribe(
        (response: any) => {
          console.log('Reset Token successfully Sent:', response);
          this.showPasswordForm = true;
        },
        (error: any) => {
          console.error('Reset Token not sent:', error);
        }
      );
  }

  submitPasswordForm() {
    const resetForm ={
      email : this.email,
      emailToken : this.emailToken,
      newPassword : this.newPassword,
      confirmPassword : this.confirmPassword
    };

    this.userDataService.resetPassword(resetForm)
      .subscribe(
      (response : any) => {
        console.log('Password Successfully Reseted', response);
      },
      (error : any) => {
        console.error('PAssword not sent: ', error);
      }
      );

    console.log('Email Token:', this.emailToken);
    console.log('New Password:', this.newPassword);
    console.log('Confirm Password:', this.confirmPassword);
    // After handling password reset, you may want to navigate to a different page or show a success message
  }
}