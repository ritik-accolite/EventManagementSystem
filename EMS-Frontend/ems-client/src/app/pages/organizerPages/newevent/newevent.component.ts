import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators , ReactiveFormsModule} from '@angular/forms';
import { UserdataService } from '../../../services/userDataService/userdata.service';
import { NgIf } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-newevent',
  standalone: true,
  imports: [ReactiveFormsModule, NgIf],
  templateUrl: './newevent.component.html',
  styleUrl: './newevent.component.css'
})
export class NeweventComponent {
  eventForm: FormGroup;
  showSuccessMessage: boolean = false;

  constructor(private fb: FormBuilder,
              private userdataservice: UserdataService,
              private router : Router) {
    this.eventForm = this.fb.group({
      eventName: ['', Validators.required],
      eventDate: ['', [Validators.required, this.futureDateValidator()]],
      eventLocation: ['', Validators.required],
      description: [''],
      eventCategory: ['', Validators.required],
      event_Time: ['', Validators.required],
      chiefGuest: ['', Validators.required],
      ticketPrice: ['', Validators.required],
      capacity: ['', Validators.required],
      bannerImage: ['',Validators.required]
    });
  }
  onSubmit() {
    if (this.eventForm.valid) {
      this.userdataservice.createEvent(this.eventForm.value).subscribe(
        (response) => {
          console.log('Event created successfully:', response);
          this.showSuccessMessage = true;
          setTimeout(() => {
            this.router.navigate(['organizer-dash','app-myevents']);          
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

