import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { UserdataService } from '../../../services/userDataService/userdata.service';
import { NgFor, NgIf } from '@angular/common';
import { JwtDecodeService } from '../../services/jwtDecodeService/jwtDecode.service';
import { Router } from '@angular/router';

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
              private jwtDecodeService: JwtDecodeService,
              private router : Router) {}

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

  eTicket(bookingId: number): void {
    this.userdataservice.getEticket(bookingId).subscribe(
      (response : any) => { 
        this.downloadFile(response);
      },
      error => console.error('Error downloading the E-ticket :',error)
    );
  }

  downloadFile(data: Blob): void {
    const blob = new Blob([data], { type: 'application/pdf' });
    const url = window.URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = url;
    link.download = 'e-ticket.pdf';
    link.click();
    window.URL.revokeObjectURL(url);
  }

  addReview(eventId: number): void{
    this.userdataservice.eventId = eventId;
    this.router.navigate(['review']);
  }

}
