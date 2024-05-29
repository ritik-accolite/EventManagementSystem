import { Component, OnInit } from '@angular/core';
import { UserdataService } from '../../../services/userDataService/userdata.service';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgFor, NgIf } from '@angular/common';
import { AllReviewsInterface } from '../../../interface/adminInterface/all-reviews-interface';

@Component({
  selector: 'app-reportedissues',
  standalone: true,
  imports: [FormsModule, NgFor, NgIf],
  templateUrl: './reportedissues.component.html',
  styleUrls: ['./reportedissues.component.css'],
})
export class ReportedissuesComponent implements OnInit {
  reviews: AllReviewsInterface[] = [];
  filteredReviews: any[] = [];
  showReported: boolean = false;
  selectedRating: number = 5;
  headingText: string = 'All Reviews';
  buttonText: string = 'Show Reported Events';

  constructor(private userdataService: UserdataService) {}

  ngOnInit() {
    this.loadReviews();
  }

  loadReviews() {
    this.userdataService.getAllReviews().subscribe((data) => {
      this.reviews = data.allReviews;
      this.filteredReviews = [...this.reviews];
    });
  }

  toggleReported() {
    this.showReported = !this.showReported;
    if (this.showReported) {
      this.filteredReviews = this.reviews.filter((review) => review.isReported);
      this.buttonText = 'Show Unreported Events';
      this.headingText = `Reported Events`;
    } else {
      this.filteredReviews = [...this.reviews];
      this.buttonText = `Show Reported Events`;
      this.headingText = `UnReported Events`;
    }
  }

  filterByRating() {
    if (this.selectedRating) {
      this.filteredReviews = this.reviews.filter(
        (review) => review.rating == this.selectedRating
      );
      this.headingText = `Events with ${this.selectedRating} ratings`;
    } else {
      this.filteredReviews = [...this.reviews];
    }
  }

  resolveReview(reviewId: string, userId: string): void {
    this.userdataService.resolveReview(reviewId, userId).subscribe(
      (response) => {
        console.log(response);
      },
      (error) => {
        console.log('Error while resolving review', error);
      }
    );
  }
}
