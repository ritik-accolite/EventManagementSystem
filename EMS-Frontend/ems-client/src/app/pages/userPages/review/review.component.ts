import { Component } from '@angular/core';
import { UserdataService } from '../../../services/userDataService/userdata.service';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-review',
  standalone: true,
  imports: [],
  templateUrl: './review.component.html',
  styleUrl: './review.component.css'
})
export class ReviewComponent {
  reviewForm: FormGroup
  eventId: number = 0;

  constructor(private fb: FormBuilder , private userdataservice: UserdataService , private route: ActivatedRoute,
    private router: Router) {
    this.reviewForm = this.fb.group({
      
    });
  }
}
