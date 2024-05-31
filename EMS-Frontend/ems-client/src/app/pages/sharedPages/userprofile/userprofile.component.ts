import { HttpClient } from '@angular/common/http';
import { Component, OnInit, inject } from '@angular/core';
import { UserdataService } from '../../../services/userDataService/userdata.service';
import { NgFor, NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { JwtDecodeService } from '../../../services/jwtDecodeService/jwtDecode.service';
import { ProfileInterface } from '../../../interface/commonInterface/profile-interface';
import { EditProfileInterface } from '../../../interface/commonInterface/edit-profile-interface';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Component({
  selector: 'app-userprofile',
  standalone: true,
  imports: [NgFor, NgIf, FormsModule],
  templateUrl: './userprofile.component.html',
  styleUrl: './userprofile.component.css',
})
export class UserprofileComponent implements OnInit {
  title = 'ems-client';
  personId: any = '';
  firstName: string = '';
  lastName: string = '';
  phoneNumber: string = '';
  editMode: boolean = false;
  showSuccessMessage: boolean = false;
  toaster=inject(ToastrService);

  constructor(
    private http: HttpClient,
    private userdataservice: UserdataService,
    private router : Router
  ) {}

  ngOnInit(): void {
    this.personId = localStorage.getItem('LoginUserId');
    this.fetchProfile(this.personId);
  }

  fetchProfile(personId: string): void {
    this.userdataservice.getProfile(personId).subscribe(
      (response: ProfileInterface) => {
        const personData = response.getPersonById;
        this.firstName = personData.firstName;
        this.lastName = personData.lastName;
        this.phoneNumber = personData.phoneNumber;
      },
      (error) => console.error('Error fetching profile :', error)
    );
  }
  toggleEditMode(): void {
    this.editMode = !this.editMode;
  }

  onSubmit(): void {
    const formData = {
      firstName: this.firstName,
      lastName: this.lastName,
      phoneNumber: this.phoneNumber,
    };
    this.userdataservice.editProfile(this.personId, formData).subscribe(
      (response: EditProfileInterface) => {
        this.toggleEditMode();
        this.toaster.success("Profile Updated Succesfully");
        try{
          const role = localStorage.getItem('Role');
          if(role =='User'){
            this.router.navigate(['user-dash','user-profile']);
          } else if (role =='Admin'){
            this.router.navigate(['admin-dash','user-profile']);
          } else if (role == 'Organizer'){
            this.router.navigate(['organizer-dash','user-profile']);
          }
          } catch{
            this.router.navigate(['/login']);
            this.toaster.error('Try Again');
          }
      },
      (error: any) => {
        console.error('Error submitting form data:', error);
      }
    );
  }
}
