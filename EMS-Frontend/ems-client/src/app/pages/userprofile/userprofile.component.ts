import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { UserdataService } from '../../userdata.service';
import { NgFor, NgIf } from '@angular/common';

@Component({
  selector: 'app-userprofile',
  standalone: true,
  imports: [NgFor , NgIf],
  templateUrl: './userprofile.component.html',
  styleUrl: './userprofile.component.css'
})
export class UserprofileComponent implements OnInit{
  title = 'ems-client';
  profiles: any[] = [];

  constructor(private http: HttpClient , private userdataservice: UserdataService) {}

  ngOnInit(): void{
    this.fetchProfile();
  }

  fetchProfile(): void {
    this.userdataservice.getProfile().subscribe(
      (response : any) => {
        this.profiles = response.allPersons;
        console.log('Profile fetched:' , this.profiles);
      },
      error => console.error('Error fetching profile :',error)
    );
  }

}
