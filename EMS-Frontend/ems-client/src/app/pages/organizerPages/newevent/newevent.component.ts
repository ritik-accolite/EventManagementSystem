import { Component } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { UserdataService } from '../../../services/userDataService/userdata.service';
import { Router } from '@angular/router';
import { NgIf } from '@angular/common';

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

  constructor(
    private fb: FormBuilder,
    private userdataservice: UserdataService,
    private router: Router
  ) {
    this.eventForm = this.fb.group({
      EventName: ['', Validators.required],
      EventDate: ['', [Validators.required, this.futureDateValidator()]],
      EventLocation: ['', Validators.required],
      Description: [''],
      EventCategory: ['', Validators.required],
      Event_Time: ['', Validators.required],
      ChiefGuest: ['', Validators.required],
      TicketPrice: ['', Validators.required],
      Capacity: ['', Validators.required],
      BannerImage: ['good'],
      BannerImageFile: [null], // Set to null initially, will be populated with file data
    });
  }

  onSubmit() {
    console.log('This is event form', this.eventForm.value);
    if (this.eventForm.valid) {
      const formData = new FormData();
      const fileInput = document.getElementById(
        'BannerImageFile'
      ) as HTMLInputElement;
      if (fileInput.files && fileInput.files.length > 0) {
        formData.append('BannerImageFile', fileInput.files[0]);
      }
      Object.keys(this.eventForm.value).forEach((key) => {
        if (key != 'BannerImageFile') {
          formData.append(key, this.eventForm.get(key)?.value);
        }
      });
      this.userdataservice.createEvent(formData).subscribe(
        (response) => {
          console.log('Event created successfully:', response);
          this.showSuccessMessage = true;
          setTimeout(() => {
            this.router.navigate(['organizer-dash', 'app-myevents']);
          }, 2000);
        },
        (error) => {
          console.error('Error creating event:', error);
        }
      );
    }
  }

  futureDateValidator() {
    return (control: any) => {
      const currentDate = new Date();
      const selectedDate = new Date(control.value);
      return selectedDate >= currentDate ? null : { futureDate: true };
    };
  }
}
