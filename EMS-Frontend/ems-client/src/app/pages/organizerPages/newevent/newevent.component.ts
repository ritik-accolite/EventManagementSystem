import { Component, inject } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
  AbstractControl,
  ValidationErrors,
} from '@angular/forms';
import { UserdataService } from '../../../services/userDataService/userdata.service';
import { Router } from '@angular/router';
import { NgIf } from '@angular/common';
import { ToastrService } from 'ngx-toastr';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-newevent',
  standalone: true,
  imports: [ReactiveFormsModule, NgIf],
  templateUrl: './newevent.component.html',
  styleUrls: ['./newevent.component.css'],
})
export class NeweventComponent {
  eventForm: FormGroup;
  showSuccessMessage: boolean = false;
  toaster = inject(ToastrService);

  constructor(
    private fb: FormBuilder,
    private userdataservice: UserdataService,
    private router: Router
  ) {
    this.eventForm = this.fb.group({
      EventName: ['', Validators.required],
      EventDate: ['', [Validators.required, this.futureDateValidator]],
      EventLocation: ['', Validators.required],
      Description: ['', Validators.required],
      EventCategory: ['', Validators.required],
      Event_Time: ['', Validators.required],
      ChiefGuest: ['', Validators.required],
      TicketPrice: ['', [Validators.required, Validators.pattern(/^[1-9][0-9]*$/)]],
      Capacity: ['', [Validators.required, Validators.pattern(/^[1-9][0-9]*$/)]],
      BannerImage: ['good'],
      BannerImageFile: [null, Validators.required], // Added required validator
    });
  }

  onSubmit() {
    if (this.eventForm.valid) {
      Swal.fire({
        title: 'Are you sure?',
        text: 'Do you want to create this event?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, create it!',
        cancelButtonText: 'No, keep editing'
      }).then((result) => {
        if (result.isConfirmed) {
          const formData = new FormData();
          const fileInput = document.getElementById(
            'BannerImageFile'
          ) as HTMLInputElement;
          if (fileInput.files && fileInput.files.length > 0) {
            formData.append('BannerImageFile', fileInput.files[0]);
          }
          Object.keys(this.eventForm.value).forEach((key) => {
            if (key !== 'BannerImageFile') {
              formData.append(key, this.eventForm.get(key)?.value);
            }
          });
          this.userdataservice.createEvent(formData).subscribe(
            (response) => {
              this.toaster.success('Event created successfully');
              setTimeout(() => {
                this.router.navigate(['organizer-dash', 'app-myevents']);
              }, 2000);
            },
            (error) => {
              console.error('Error creating event:', error);
            }
          );
        }
      });
    } else {
      this.eventForm.markAllAsTouched();
    }
  }

  futureDateValidator(control: AbstractControl): ValidationErrors | null {
    const currentDate = new Date();
    const selectedDate = new Date(control.value);
    return selectedDate >= currentDate ? null : { futureDate: true };
  }
}
