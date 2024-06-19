import { Component, OnInit, inject } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { UserdataService } from '../../../services/userDataService/userdata.service';
import { JwtDecodeService } from '../../../services/jwtDecodeService/jwtDecode.service';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule, DatePipe, NgIf } from '@angular/common';
import { ToastrService } from 'ngx-toastr';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-eventbooking',
  standalone: true,
  imports: [ReactiveFormsModule, NgIf, CommonModule, DatePipe],
  templateUrl: './eventbooking.component.html',
  styleUrls: ['./eventbooking.component.css'],
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
  showPaymentPopup: boolean = false;
  eventDetails: any;
  toaster = inject(ToastrService);
  validatePayment: boolean = false;
  token: string = '';
  invalid: boolean = false; // Add this line

  constructor(
    private fb: FormBuilder,
    private userdataservice: UserdataService,
    private jwtDecodeService: JwtDecodeService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.bookEventForm = this.fb.group({
      numberOfTickets: [
        '',
        [Validators.required, Validators.min(1), Validators.max(5)],
      ],
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

    this.userdataservice.getEventById(this.eventId).subscribe(
      (eventDetails: any) => {
        this.eventDetails = eventDetails.getEventById;
      },
      (error) => {
        console.error('Error fetching event details:', error);
      }
    );
  }

  paymentDone() {
    this.userdataservice.successPayment(this.token).subscribe(
      (response: any) => {
        if (response.status == 200) {
          this.router.navigate(['user-dash', 'mybookings']);
          this.toaster.info('Event Ticket Successfully booked');
        } else if (response.status == 404) {
          this.toaster.info('Event not Found. Try Again');
        } else if (response.status == 400) {
          this.toaster.info('You have already booked tickets for this event');
        } else if (response.status == 401) {
          this.toaster.info('Not enough tickets available for this event');
        } else if (response.status == 402) {
          this.toaster.info('You cannot book more than 5 tickets.');
        } else if (response.status == 500) {
          this.toaster.error('Error Unbooking Event. Try Again!');
        }
      },
      (error: any) => {
        console.log(error);
      }
    );
  }

  onSubmit() {
    if (this.bookEventForm.valid) {
      const formData = {
        eventId: this.eventId,
        userId: this.id,
        numberOfTickets: this.bookEventForm.value.numberOfTickets,
      };

      this.userdataservice
        .createCheckoutSession(
          formData.eventId,
          formData.userId,
          formData.numberOfTickets
        )
        .subscribe(
          (response: any) => {
            window.open(response.url, '_blank'); // Open Stripe checkout in a new tab
            this.token = response.token;
            this.validatePayment = true;

            // Show confirmation box after opening Stripe checkout
            Swal.fire({
              title: 'Payment Confirmation',
              text: 'Have you completed the payment on Stripe?',
              icon: 'info',
              showCancelButton: true,
              confirmButtonText: 'Yes, payment done!',
              cancelButtonText: 'No, not yet'
            }).then((result) => {
              if (result.isConfirmed) {
                this.paymentDone();
              }
            });
          },
          (error) => {
            this.toaster.error('Error creating checkout session', 'Error');
            console.error('Error creating checkout session', error);
          }
        );
    } else {
      this.invalid = true;
    }
  }
  

  calculatePrice() {
    const numberOfTickets = this.bookEventForm.get('numberOfTickets')?.value;
    this.totalPrice = numberOfTickets * this.ticketPrice;
  }
}
