import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { UserdataService } from '../../userdata.service';
import { NgFor, NgIf } from '@angular/common';
import { JwtDecodeService } from '../../services/jwtDecode.service';

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

  constructor(private http: HttpClient , private userdataservice: UserdataService) {}

  ngOnInit(): void {
      this.fetchUserBookedEvents();
  }

  fetchUserBookedEvents(): void{
    
    this.userdataservice.getUserEvents()
      .subscribe((response : any) => {
        this.bookedEvents  = response;
        console.log('Booked events fetched:', this.bookedEvents);
      },
    error => console.error('Error fetching booked events :', error));
  }
}
