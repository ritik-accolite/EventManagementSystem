import { BookedEventDetailsInterface } from './booked-event-details-interface';

export interface UserEventsInterface {
  Status: number;
  Message: string;
  bookedEvents: BookedEventDetailsInterface[];
}
