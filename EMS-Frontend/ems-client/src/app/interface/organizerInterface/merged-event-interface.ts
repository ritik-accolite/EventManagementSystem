import { AllEventInterface } from '../commonInterface/all-event-interface';
import { EventTicketInterface } from './event-ticket-status';

export interface MergedEventInterface
  extends AllEventInterface,
    EventTicketInterface {}
