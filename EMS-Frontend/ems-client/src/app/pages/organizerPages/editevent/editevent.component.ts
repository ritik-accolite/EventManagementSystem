import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { UserdataService } from '../../../services/userDataService/userdata.service';
import { Router } from '@angular/router';
import { EventDetailsInterface } from '../../../interface/organizerInterface/event-details-interface';
import { EventDetailsByIdInterface } from '../../../interface/organizerInterface/event-details-by-id-interface';
import { UpdateEventInterface } from '../../../interface/organizerInterface/update-event-interface';
import { ResponseInterface } from '../../../interface/commonInterface/response-interface';
import { ToastrService } from 'ngx-toastr';

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
  toaster=inject(ToastrService);

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
      this.userdataService.getEventDetails(this.eventId).subscribe((eventDetails: EventDetailsByIdInterface) => {
        // Prefill form with fetched event details
        console.log("prefil details",eventDetails.getEventById);
        this.eventForm.patchValue(eventDetails.getEventById);
      },
      (error) => {
        console.error("Error fetching event details:", error);
      });
    }
  }

  onSubmit() {
    if (this.eventForm.valid) {
      // Send a POST request with edited form data to update event details
      this.toaster.success("Succesfully Submitted","Success");
      this.userdataService.updateEvent(this.eventId, this.eventForm.value).subscribe((response: any) => {
        this.router.navigate(['organizer-dash','app-myevents']);
      });
    }
  }

  onDelete() {
    if (confirm("Are you sure you want to delete this event?")) {
      this.toaster.info("Deleted Succesfully");
      this.userdataService.deleteEvent(this.eventId).subscribe((response: any) => {
        this.router.navigate(['user-dash','/app-myevents']);
      });
    }
}
}
