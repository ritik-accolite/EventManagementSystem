import { AllReviewsInterface } from './all-reviews-interface';

export interface GetAllReviewsInterface {
  status: number;
  message: string;
  allReviews: AllReviewsInterface[];
}
