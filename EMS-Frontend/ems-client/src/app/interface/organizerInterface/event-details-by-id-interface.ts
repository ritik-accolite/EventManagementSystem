import { PersonInterface } from "../commonInterface/person-interface";

export interface EventDetailsByIdInterface {
    status: number;
    message: string;
    getEventById: {
      eventId: number;
      eventName: string;
      eventCategory: string;
      description: string;
      chiefGuest: string;
      eventDate: string;
      event_Time: string;
      eventLocation: string;
      ticketPrice: number;
      capacity: number;
      bannerImage: string;
      eventOrganizerId: string;
      organizer: PersonInterface;
    };
  }