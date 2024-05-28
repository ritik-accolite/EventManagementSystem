export interface EventTicketInterface {
  eventId: number;
  eventName: string;
  totalTicketsSold: number;
  totalTicketsLeft: number;
  ticketPrice: number;
  userTickets: UserTicket[];
}

export interface UserTicket {
  username: string;
  totalTicketsBooked: number;
  totalPayable: number;
}

export interface EventTicketStatus {
  events: EventTicketInterface[];
}
