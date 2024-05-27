import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { UserdataService } from '../../../services/userDataService/userdata.service';
import { DatePipe, NgFor, NgIf } from '@angular/common';
import { JwtDecodeService } from '../../../services/jwtDecodeService/jwtDecode.service';
import { Router, RouterLink } from '@angular/router';
import { UserEventsInterface } from '../../../interface/userInterface/user-events-interface';
import { BookedEventDetailsInterface } from '../../../interface/userInterface/booked-event-details-interface';

@Component({
  selector: 'app-mybookings',
  standalone: true,
  imports: [NgIf, NgFor, RouterLink, DatePipe],
  templateUrl: './mybookings.component.html',
  styleUrl: './mybookings.component.css',
})
export class MybookingsComponent implements OnInit {
  title = 'ems-client';
  role = '';
  disabled = false;
  bookedEvents: BookedEventDetailsInterface[] = [];

  constructor(
    private userdataservice: UserdataService,
    private jwtDecodeService: JwtDecodeService
  ) {}

  ngOnInit(): void {
    this.fetchUserBookedEvents();
  }

  fetchUserBookedEvents(): void {
    if (this.jwtDecodeService.userId != '') {
      this.userdataservice
        .getUserEventsId(this.jwtDecodeService.userId)
        .subscribe(
          (response: UserEventsInterface) => {
            this.bookedEvents = response.bookedEvents;
            this.role = this.jwtDecodeService.role;
            if (this.role != 'User') {
              this.disabled = true;
            }
          },
          (error) => console.error('Error fetching booked events :', error)
        );
    } else {
      this.userdataservice.getUserEvents().subscribe(
        (response: UserEventsInterface) => {
          this.bookedEvents = response.bookedEvents;
        },
        (error) => console.error('Error fetching booked events :', error)
      );
    }
  }

  eTicket(bookingId: number): void {
    this.userdataservice.getEticket(bookingId).subscribe(
      (response: any) => {
        this.downloadFile(response);
      },
      (error) => console.error('Error downloading the E-ticket :', error)
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

  addReview(eventId: number): void {
    this.userdataservice.eventId = eventId;
    console.log(eventId);
  }
}
