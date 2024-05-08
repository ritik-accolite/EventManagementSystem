import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators , ReactiveFormsModule} from '@angular/forms';
import { UserdataService } from '../../userdata.service';
import { NgIf } from '@angular/common';

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

  constructor(private fb: FormBuilder, private userdataservice: UserdataService) {
    this.eventForm = this.fb.group({
      eventName: ['', Validators.required],
      eventDate: ['', Validators.required],
      eventLocation: ['', Validators.required],
      description: [''],
      eventCategory: ['', Validators.required],
      event_Time: ['', Validators.required],
      chiefGuest: ['', Validators.required],
      ticketPrice: ['', Validators.required],
      capacity: ['', Validators.required],
      bannerImage: []
    });
  }
  onSubmit() {
    if (this.eventForm.valid) {
      this.userdataservice.createEvent(this.eventForm.value).subscribe(
        (response) => {
          console.log('Event created successfully:', response);
          this.showSuccessMessage = true;
          // this.router.navigate(['/user-dash']);
          
        },
        (error) => {
          console.error('Error creating event:', error);
        }
      );
    }
  }

}

