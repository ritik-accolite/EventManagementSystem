import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { UserdataService } from '../../../services/userDataService/userdata.service';
import { JwtDecodeService } from '../../../services/jwtDecodeService/jwtDecode.service';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule, NgIf } from '@angular/common';

@Component({
  selector: 'app-eventbooking',
  standalone: true,
  imports: [ReactiveFormsModule, NgIf, CommonModule],
  templateUrl: './eventbooking.component.html',
  styleUrl: './eventbooking.component.css',
})
export class EventbookingComponent implements OnInit {
  bookEventForm: FormGroup;
  role: string = '';
  id: string = '';
  organizerId: string = '';
  eventId: number = 0;
  ticketPrice: number = 0;
  bookingMessage: string = '';
  status: number = 0;
  totalPrice: number = 0;
  paymentForm: FormGroup;
  showPaymentPopup: boolean = false;

  constructor(
    private fb: FormBuilder,
    private userdataservice: UserdataService,
    private jwtDecodeService: JwtDecodeService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.bookEventForm = this.fb.group({
      numberOfTickets: ['', Validators.required],
    });

    this.paymentForm = this.fb.group({
      cardNumber: ['', Validators.required],
      expiryDate: ['', Validators.required],
      cvv: ['', Validators.required],
    });
  }

  ngOnInit(): void {
    this.role = this.jwtDecodeService.role;
    this.id = this.jwtDecodeService.id;
    this.route.params.subscribe((params) => {
      this.eventId = this.userdataservice.eventId;
      this.organizerId = this.userdataservice.organizerId;
      this.ticketPrice = this.userdataservice.ticketPrice;
    });
  }

  openPaymentPopup() {
    this.showPaymentPopup = true;
  }

  onSubmit() {
    if (this.bookEventForm.valid) {
      const formData = {
        ...this.bookEventForm.value,
        eventId: this.eventId,
        EventOrganizerId: this.organizerId,
        UserId: this.id,
      };

      this.userdataservice.bookEvent(formData).subscribe(
        (response) => {
          this.status = response.status;
          if (this.status === 200) {
            this.bookingMessage = response.message;
            this.showPaymentPopup = false;
            this.router.navigate(['mybookings']);
          } else {
            this.bookingMessage = 'Error: ' + response.message;
            this.showPaymentPopup = false;
          }
          console.log('Booked event successfully', response);
        },
        (error) => {
          this.status = error.status || 500;
          this.bookingMessage =
            'Error: ' + (error.error.message || 'Unknown error');
          console.error('Error booking event', error);
          this.showPaymentPopup = false;
        }
      );
    }
  }
  calculatePrice() {
    const numberOfTickets = this.bookEventForm.get('numberOfTickets')?.value;
    this.totalPrice = numberOfTickets * this.ticketPrice;
  }
}
