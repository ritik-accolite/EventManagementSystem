import { Component, OnInit } from '@angular/core';
import { UserdataService } from '../../../services/userDataService/userdata.service';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgFor, NgIf } from '@angular/common';
import { AllReviewsInterface } from '../../../interface/adminInterface/all-reviews-interface';
import { ToastrService } from 'ngx-toastr'; // Add Toastr for notification
import Swal from 'sweetalert2'; // Import SweetAlert2

@Component({
  selector: 'app-reportedissues',
  standalone: true,
  imports: [FormsModule, NgFor, NgIf],
  templateUrl: './reportedissues.component.html',
  styleUrls: ['./reportedissues.component.css'],
})
export class ReportedissuesComponent implements OnInit {
  name: string = '';
  reviews: AllReviewsInterface[] = [];
  filteredReviews: any[] = [];
  showReported: boolean = false;
  selectedRating: number = 5;
  headingText: string = 'All Reviews';
  buttonText: string = 'Show Reported Events';

  constructor(private userdataService: UserdataService, private toastr: ToastrService) {}

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

  confirmResolveReview(reviewId: string, userId: string): void {
    Swal.fire({
      title: 'Are you sure?',
      html: `
        <p>Do you really want to mark this issue as resolved?</p>
        <label for="resolutionDescription">Please provide a resolution description:</label>
        <input type="text" id="resolutionDescription" class="swal2-input" placeholder="Resolution description">
      `,
      icon: 'info',
      showCancelButton: true,
      confirmButtonText: 'Yes, resolve it!',
      cancelButtonText: 'No, cancel',
      preConfirm: () => {
        const resolutionDescription = (document.getElementById('resolutionDescription') as HTMLInputElement).value;
        if (!resolutionDescription) {
          Swal.showValidationMessage('Resolution description is required');
          return false;
        }
        return { resolutionDescription };
      }
    }).then((result) => {
      if (result.isConfirmed) {
        const description = result.value?.resolutionDescription;
        this.resolveReview(reviewId, userId);
      }
    });
  }

  resolveReview(reviewId: string, userId: string): void {
    this.userdataService.resolveReview(reviewId, userId).subscribe(
      (response) => {
        this.toastr.success('Issue resolved successfully.');
        this.loadReviews(); // Refresh the list after resolving
      },
      (error) => {
        this.toastr.error('Error while resolving review');
        console.log('Error while resolving review', error);
      }
    );
  }
}
