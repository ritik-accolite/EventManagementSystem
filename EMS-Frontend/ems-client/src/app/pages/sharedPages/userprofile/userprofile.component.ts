import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { UserdataService } from '../../../services/userDataService/userdata.service';
import { NgFor, NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { JwtDecodeService } from '../../../services/jwtDecodeService/jwtDecode.service';
import { ProfileInterface } from '../../../interface/commonInterface/profile-interface';
import { EditProfileInterface } from '../../../interface/commonInterface/edit-profile-interface';

@Component({
  selector: 'app-userprofile',
  standalone: true,
  imports: [NgFor , NgIf, FormsModule],
  templateUrl: './userprofile.component.html',
  styleUrl: './userprofile.component.css'
})
export class UserprofileComponent implements OnInit{
  title = 'ems-client';
  personId: any = '';
  firstName : string ='';
  lastName : string ='';
  phoneNumber : string ='';
  editMode: boolean = false;
  showSuccessMessage: boolean = false;

  constructor(private http: HttpClient , private userdataservice: UserdataService,
              private jwtDecodeService : JwtDecodeService
  ) {}

  ngOnInit(): void{
    // this.personId = this.jwtDecodeService.id;
    this.personId = localStorage.getItem('LoginUserId');
    console.log(this.personId);
    this.fetchProfile(this.personId);
  }

  fetchProfile(personId : string): void {
    this.userdataservice.getProfile(personId).subscribe(
      (response : ProfileInterface) => {
        const personData = response.getPersonById;
        this.firstName = personData.firstName;
        this.lastName = personData.lastName;
        this.phoneNumber = personData.phoneNumber;
      },
      error => console.error('Error fetching profile :',error)
    );
  }
  toggleEditMode(): void {
    this.editMode = !this.editMode;
  }

  onSubmit(): void {
    const formData = {
      firstName: this.firstName,
      lastName: this.lastName,
      phoneNumber: this.phoneNumber
    };
    this.userdataservice.editProfile(this.personId, formData)
    .subscribe(
      (response: EditProfileInterface) => {
        console.log('Update successful:', response);
        this.showSuccessMessage = true;
        
      },
      (error: any) => {
        console.error('Error submitting form data:', error);
      }
    );
  }

  
}
