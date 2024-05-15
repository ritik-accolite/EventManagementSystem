import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { UserdataService } from '../../services/userDataService/userdata.service';
import { NgFor, NgIf } from '@angular/common';
import { JwtDecodeService } from '../../services/jwtDecodeService/jwtDecode.service';

@Component({
  selector: 'app-mybookings',
  standalone: true,
  imports: [NgIf , NgFor],
  templateUrl: './mybookings.component.html',
  styleUrl: './mybookings.component.css'
})
export class MybookingsComponent implements OnInit{
  title = 'ems-client';
  bookedEvents: any[] = [];

  constructor(private http: HttpClient ,
              private userdataservice: UserdataService,
              private jwtDecodeService: JwtDecodeService) {}

  ngOnInit(): void {
      this.fetchUserBookedEvents();
  }

  fetchUserBookedEvents(): void{
    if(this.jwtDecodeService.userId != ''){
      this.userdataservice.getUserEventsId(this.jwtDecodeService.userId)
      .subscribe((response : any) => {
        this.bookedEvents  = response.bookedEvents;
        console.log('Booked events fetched 123:', this.bookedEvents);
      },
    error => console.error('Error fetching booked events :', error));
    } else {
    this.userdataservice.getUserEvents()
      .subscribe((response : any) => {
        this.bookedEvents  = response.bookedEvents;
        console.log('Booked events fetched 123:', this.bookedEvents);
      },
    error => console.error('Error fetching booked events :', error));
  }
}
}
