import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, AbstractControl, ValidationErrors } from '@angular/forms';
import { UserdataService } from '../../../services/userDataService/userdata.service';
import { Router } from '@angular/router';
import { EventDetailsByIdInterface } from '../../../interface/organizerInterface/event-details-by-id-interface';
import { ToastrService } from 'ngx-toastr';
import Swal from 'sweetalert2';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-editevent',
  standalone: true,
  imports: [ReactiveFormsModule, NgIf],
  templateUrl: './editevent.component.html',
  styleUrls: ['./editevent.component.css'],
})
export class EditeventComponent implements OnInit {
  eventForm: FormGroup;
  eventId: any;
  toaster = inject(ToastrService);

  constructor(
    private fb: FormBuilder,
    private userdataService: UserdataService,
    private router: Router
  ) {
    this.eventForm = this.fb.group({
      eventName: ['', Validators.required],
      eventDate: ['', [Validators.required, this.futureDateValidator, this.editableDateValidator]],
      eventLocation: ['', Validators.required],
      description: [''],
      eventCategory: ['', Validators.required],
      event_Time: ['', Validators.required],
      chiefGuest: ['', Validators.required],
      ticketPrice: ['', [Validators.required, Validators.pattern(/^[1-9][0-9]*$/)]],
      capacity: ['', [Validators.required, Validators.pattern(/^[1-9][0-9]*$/)]],
      bannerImage: [],
    });
  }

  ngOnInit(): void {
    this.eventId = this.userdataService.eventId;
    if (this.eventId) {
      this.userdataService.getEventDetails(this.eventId).subscribe(
        (eventDetails: EventDetailsByIdInterface) => {
          // Prefill form with fetched event details
          console.log('prefill details', eventDetails.getEventById);
          this.eventForm.patchValue(eventDetails.getEventById);
        },
        (error) => {
          console.error('Error fetching event details:', error);
        }
      );
    }
  }

  onSubmit() {
    if (this.eventForm.valid) {
      Swal.fire({
        title: 'Are you sure?',
        text: 'Do you want to submit the changes?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, submit it!',
        cancelButtonText: 'No, keep editing'
      }).then((result) => {
        if (result.isConfirmed) {
          // Send a POST request with edited form data to update event details
          this.userdataService.updateEvent(this.eventId, this.eventForm.value).subscribe(
            (response: any) => {
              this.toaster.success('Successfully Submitted', 'Success');
              this.router.navigate(['organizer-dash', 'app-myevents']);
            },
            (error: any) => {
              console.error('Error updating event:', error);
            }
          );
        }
      });
    } else {
      this.eventForm.markAllAsTouched(); // To show validation errors
    }
  }

  onDelete() {
    Swal.fire({
      title: 'Are you sure?',
      html: `
        <p>You are about to delete this event. Please note the following:</p>
        <ul>
          <li>All registered attendees will be notified.</li>
          <li>Refunds will be processed automatically within 7 business days.</li>
          <li>This action cannot be undone.</li>
        </ul>
        <p>Do you really want to proceed?</p>
      `,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Yes, delete it!',
      cancelButtonText: 'No, keep it'
    }).then((result) => {
      if (result.isConfirmed) {
        this.userdataService.deleteEvent(this.eventId).subscribe(
          (response: any) => {
            this.toaster.info('Deleted Successfully');
            this.router.navigate(['organizer-dash', 'app-myevents']);
          },
          (error: any) => {
            console.error('Error deleting event:', error);
          }
        );
      }
    });
  }

  futureDateValidator(control: AbstractControl): ValidationErrors | null {
    const currentDate = new Date();
    const selectedDate = new Date(control.value);
    return selectedDate >= currentDate ? null : { futureDate: true };
  }

  editableDateValidator(control: AbstractControl): ValidationErrors | null {
    const currentDate = new Date();
    const selectedDate = new Date(control.value);
    const diffInDays = (selectedDate.getTime() - currentDate.getTime()) / (1000 * 3600 * 24);
    return diffInDays > 3 ? null : { editableDate: true };
  }
}
