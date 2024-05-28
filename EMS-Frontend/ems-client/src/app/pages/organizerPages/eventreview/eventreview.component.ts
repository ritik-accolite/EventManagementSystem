import { Component } from '@angular/core';
import { UserdataService } from '../../../services/userDataService/userdata.service';
import { FormsModule } from '@angular/forms';
import { NgFor, NgIf } from '@angular/common';

@Component({
  selector: 'app-eventreview',
  standalone: true,
  imports: [FormsModule, NgIf, NgFor],
  templateUrl: './eventreview.component.html',
  styleUrl: './eventreview.component.css',
})
export class EventreviewComponent {
  reviews: any[] = [];
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
    this.userdataService
      .getAllReviewsByEventId(this.userdataService.eventId)
      .subscribe((data) => {
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
      this.buttonText = 'Show Reported Events';
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
}
