import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { UserdataService } from '../../../services/userDataService/userdata.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-editevent',
  standalone : true,
  imports: [ReactiveFormsModule],
  templateUrl: './editevent.component.html',
  styleUrls: ['./editevent.component.css']
})
export class EditeventComponent implements OnInit {
  eventForm: FormGroup;
  eventId: any;

  constructor(
    private fb: FormBuilder,
    private userdataService: UserdataService,
    private router : Router
  ) {
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

  ngOnInit(): void {
    this.eventId = this.userdataService.eventId;
    if (this.eventId) {
      this.userdataService.getEventDetails(this.eventId).subscribe((eventDetails: any) => {
        // Prefill form with fetched event details
        console.log("prefil details",eventDetails['getEventById']);
        this.eventForm.patchValue(eventDetails['getEventById']);
      },
      (error) => {
        console.error("Error fetching event details:", error);
      });
    }
  }

  onSubmit() {
    if (this.eventForm.valid) {
      // Send a POST request with edited form data to update event details
      this.userdataService.updateEvent(this.eventId, this.eventForm.value).subscribe((response: any) => {
        this.router.navigate(['organizer-dash','app-myevents']);
      });
    }
  }

  onDelete() {
    if (confirm("Are you sure you want to delete this event?")) {
      this.userdataService.deleteEvent(this.eventId).subscribe((response: any) => {
        console.log('Response while deleting: ', response);
        this.router.navigate(['organizer-dash','/app-myevents']);
      });
    }
}
}
