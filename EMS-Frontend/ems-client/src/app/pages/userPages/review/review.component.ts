import { Component, OnInit, inject } from '@angular/core';
import { UserdataService } from '../../../services/userDataService/userdata.service';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { JwtDecodeService } from '../../../services/jwtDecodeService/jwtDecode.service';
import { ReviewInterface } from '../../../interface/userInterface/review-interface';
import { ToastrService } from 'ngx-toastr';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-review',
  standalone: true,
  imports: [ReactiveFormsModule, NgIf],
  templateUrl: './review.component.html',
  styleUrl: './review.component.css',
})
export class ReviewComponent implements OnInit {
  reviewForm: FormGroup;
  eventId: number = 0;
  id: string = '';
  status: number = 0;
  toaster=inject(ToastrService);
  invalid : boolean = false;

  constructor(
    private fb: FormBuilder,
    private userdataservice: UserdataService,
    private route: ActivatedRoute,
    private router: Router,
    private jwtDecodeService: JwtDecodeService
  ) {
    this.reviewForm = this.fb.group({
      Description: ['', Validators.required],
      Rating: ['', [Validators.required, Validators.min(1), Validators.max(5)]],
      IsReported: [false],
    });
  }

  ngOnInit(): void {
    this.id = this.jwtDecodeService.id;
    this.route.params.subscribe((params) => {
      this.eventId = this.userdataservice.eventId;
    });
  }

  onSubmit() {
    if (this.reviewForm.valid) {
      const formData = {
        ...this.reviewForm.value,
        eventId: this.eventId,
        UserId: this.id,
      };
      this.userdataservice.addReview(formData).subscribe(
        (response) => {
          this.status = response.status;
          if (this.status === 200) {
            this.router.navigate(['user-dash','mybookings']);
            this.toaster.success("Review Successfully Submitted");
          }
          if (this.status === 400) {
            this.router.navigate(['user-dash','mybookings']);
            this.toaster.info("You have already added review for this event.");
          }
          console.log(response);
        },
        (error) => {
          console.error('Error adding event', error);
        }
      );
    }
    else{
      this.invalid = true;
    }
  }
}
