import { EventUserInterface } from './event-user-interface';

export interface EventByCategoriesInterface {
  status: number;
  message: string;
  categoryEvents: EventUserInterface[];
}
