import { BookedUserInterface } from "./booked-user-interface";

export interface EventDetailsInterface {
    eventId :  number,
    eventName : string,
    eventCategory : string,
    description : string,
    chiefGuest : string,
    eventDate : Date,
    eventTime :  string,
    eventLocation : string,
    ticketPrice : number,
    capacity : number,
    bannerImage : string,
    bookedUsers : BookedUserInterface
}
