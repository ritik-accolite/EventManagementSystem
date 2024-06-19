import { Component, OnInit, inject } from '@angular/core';
import { UserdataService } from '../../../services/userDataService/userdata.service';
import { DatePipe, NgFor, NgIf } from '@angular/common';
import { JwtDecodeService } from '../../../services/jwtDecodeService/jwtDecode.service';
import { Router, RouterLink } from '@angular/router';
import { UserEventsInterface } from '../../../interface/userInterface/user-events-interface';
import { BookedEventDetailsInterface } from '../../../interface/userInterface/booked-event-details-interface';
import { ToastrService } from 'ngx-toastr';
import { response } from 'express';
import { error } from 'console';

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
  toaster=inject(ToastrService);

  constructor(
    private userdataservice: UserdataService,
    private jwtDecodeService: JwtDecodeService,
    private router : Router
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
  viewTicket(bookingId : number, eventId : number){
    this.userdataservice.eventId = eventId;
    this.userdataservice.bookingId = bookingId;
    this.router.navigate(['user-dash','user-ticket']);
   }
   isEventDateInPast(date : Date): boolean {
    const eventDate = new Date(date);
    const currentDate = new Date();
    return eventDate <= currentDate;
  }
   
   cancelBooking(bookingId: number){
    this.userdataservice.cancelEvent(bookingId).subscribe(
      (response: any) => {
        if(response.status == 200){
          this.router.navigate(['user-dash','event-list']);
          this.toaster.success('Event Unbooked Successfully.');
        } else if (response.status == 404){
          this.toaster.info('Event not Found. Try Again');
        } else if (response.status == 500){
          this.toaster.error('Error Unbooking Event. Try Again!');
        }
      },
      (error: any) => {
        this.toaster.error('Error Unbooking Event. Try Again!');
        console.log(error);
      }
    )
   }

  // eTicket(bookingId: number): void {
  //   this.userdataservice.getEticket(bookingId).subscribe(
  //     (response: any) => {
  //       this.toaster.success("Downloaded Successfully!");
  //       this.downloadFile(response);
        
  //     },
  //     (error) => console.error('Error downloading the E-ticket :', error)
  //   );
  // }

  downloadFile(data: Blob): void {
    const blob = new Blob([data], { type: 'application/pdf' });
    const url = window.URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = url;
    link.download = 'e-ticket.pdf';
    link.click();
    window.URL.revokeObjectURL(url);
    this.toaster.info("Downloaded Successfully!");
  }

  addReview(eventId: number): void {
    this.userdataservice.eventId = eventId;
    console.log(eventId);
  }
}
