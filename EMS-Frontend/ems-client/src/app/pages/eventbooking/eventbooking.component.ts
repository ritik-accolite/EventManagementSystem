import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { UserdataService } from '../../userdata.service';
import { NgIf } from '@angular/common';
import { JwtDecodeService } from '../../services/jwtDecode.service';

@Component({
  selector: 'app-eventbooking',
  standalone: true,
  imports: [ReactiveFormsModule , NgIf],
  templateUrl: './eventbooking.component.html',
  styleUrl: './eventbooking.component.css'
})
export class EventbookingComponent implements OnInit{
  bookEventForm: FormGroup;

  role : string = '';

  ngOnInit(): void{
    this.role = this.jwtDecodeService.role;
  }

    constructor(private fb: FormBuilder, private userdataservice: UserdataService , private jwtDecodeService : JwtDecodeService) {
      this.bookEventForm = this.fb.group({
        bookingDate: ['',Validators.required],
        numberOfTickets: ['',Validators.required]
      });
    }
    onSubmit() {
      if (this.bookEventForm.valid) {
        this.userdataservice.bookEvent(this.bookEventForm.value).subscribe(
          (response) => {
            console.log('Booked event successfully',response);
          },
          (error) => {
            console.error('Error booking event',error);
          }
        );
      }
    }
}
