import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { UserdataService } from '../../../services/userDataService/userdata.service';
import { NgFor, NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { JwtDecodeService } from '../../../services/jwtDecodeService/jwtDecode.service';

export interface Person {
  accessFailedCount: number;
  concurrencyStamp: string;
  email: string;
  emailConfirmed: boolean;
  firstName: string;
  id: string;
  lastName: string;
  lockoutEnabled: boolean;
  lockoutEnd: Date | null;
  normalizedEmail: string;
  normalizedUserName: string;
  password: string;
  passwordHash: string;
  phoneNumber: string;
  phoneNumberConfirmed: boolean;
  role: string;
  securityStamp: string;
  twoFactorEnabled: boolean;
  userName: string;
}


@Component({
  selector: 'app-userprofile',
  standalone: true,
  imports: [NgFor , NgIf, FormsModule],
  templateUrl: './userprofile.component.html',
  styleUrl: './userprofile.component.css'
})
export class UserprofileComponent implements OnInit{
  title = 'ems-client';
  personId: string = '';
  firstName : string ='';
  lastName : string ='';
  phoneNumber : string ='';
  editMode: boolean = false;
  showSuccessMessage: boolean = false;

  constructor(private http: HttpClient , private userdataservice: UserdataService,
              private jwtDecodeService : JwtDecodeService
  ) {}

  ngOnInit(): void{
    this.personId = this.jwtDecodeService.id;
    this.fetchProfile(this.personId);
  }

  fetchProfile(personId : string): void {
    this.userdataservice.getProfile(personId).subscribe(
      (response : any) => {
        const personData: Person = response.getPersonById;
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
      (response: any) => {
        console.log('Update successful:', response);
        this.showSuccessMessage = true;
        
      },
      (error: any) => {
        console.error('Error submitting form data:', error);
      }
    );
  }

  
}
