import { PersonInterface } from "../commonInterface/person-interface";

export interface EventUserInterface {
    eventId : number,
    eventName : string,
    eventCategory : string,
    description : string,
    chiefGuest : string,
    eventDate : Date,
    eventTime : string,
    eventLocation : string,
    ticketPrice : number,
    capacity : number,
    bannerImage : string,
    eventOrganizerId : string,
    organizer : PersonInterface
}
