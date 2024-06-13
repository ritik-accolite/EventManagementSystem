import { Component, OnInit } from '@angular/core';
import { UserdataService } from '../../../services/userDataService/userdata.service'; 
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { DatePipe } from '@angular/common';
import { jsPDF } from 'jspdf';
import html2canvas from 'html2canvas';
@Component({
  selector: 'app-userticket',
  standalone: true,
  imports: [RouterLink, DatePipe],
  templateUrl: './userticket.component.html',
  styleUrl: './userticket.component.css',
})
export class UserticketComponent implements OnInit {
  eventId: number = 0;
  bookingId: number = 0;
  eventDetails: any;
  userId: string = '';
  currentDate: Date = new Date();

  constructor(
    private http: HttpClient,
    private userdataService: UserdataService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.bookingId = this.userdataService.bookingId;
    console.log('booking id', this.bookingId);

    if (this.bookingId) {
      this.userdataService.getETicketDetailsById(this.bookingId).subscribe(
        (eventDetails: any) => {
          console.log('event response', eventDetails);
          this.eventDetails = eventDetails.eTicket;
        
        },
        (error) => console.error('Error downloading the E-ticket :', error)
      );
    }
  }
  generatePDF(): void {
    console.log('in pdf');
    const data = document.getElementById('invoice');
    if (data) {
      console.log('in pdf condition');
      html2canvas(data).then((canvas) => {
        const imgWidth = 208;
        const imgHeight = (canvas.height * imgWidth) / canvas.width;
        const contentDataURL = canvas.toDataURL('image/png');
        let pdf = new jsPDF('p', 'mm', 'a4');
        pdf.addImage(contentDataURL, 'PNG', 0, 0, imgWidth, imgHeight);
        pdf.save(`${this.eventDetails.eventName}_${this.eventDetails.bookingId}.pdf`);
      });
    }
  }
}
