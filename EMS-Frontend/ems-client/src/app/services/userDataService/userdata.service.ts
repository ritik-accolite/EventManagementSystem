import { EventEmitter, Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ReviewInterface } from '../../interface/userInterface/review-interface';
import { EventBookingInterface } from '../../interface/userInterface/event-booking-interface';
import { RegisterInterface } from '../../interface/commonInterface/register-interface';
import { LoginInterface } from '../../interface/commonInterface/login-interface';
import { CreateEventInterface } from '../../interface/organizerInterface/create-event-interface';
import { EmailNotificationsInterface } from '../../interface/commonInterface/email-notifications-interface';
import { EventInterface } from '../../interface/commonInterface/event-interface';
import { OrganizerEventInterface } from '../../interface/organizerInterface/organizer-event-interface';
import { ProfileInterface } from '../../interface/commonInterface/profile-interface';
import { UserEventsInterface } from '../../interface/userInterface/user-events-interface';
import { EventDetailsInterface } from '../../interface/organizerInterface/event-details-interface';
import { GetPersonByRoleInterface } from '../../interface/adminInterface/get-person-by-role-interface';
import { GetAllReviewsInterface } from '../../interface/adminInterface/get-all-reviews-interface';
import { EventTicketStatus } from '../../interface/organizerInterface/event-ticket-status';
import { EventDetailsByIdInterface } from '../../interface/organizerInterface/event-details-by-id-interface';
import { EventByCategoriesInterface } from '../../interface/userInterface/event-by-categories-interface';
import { ForgetPasswordInterface } from '../../interface/commonInterface/forget-password-interface';
import { EditProfileInterface } from '../../interface/commonInterface/edit-profile-interface';
import { UpdateEventInterface } from '../../interface/organizerInterface/update-event-interface';
import { ResponseInterface } from '../../interface/commonInterface/response-interface';

@Injectable({
  providedIn: 'root'
})
export class UserdataService {
  eventId : number = 0;
  organizerId: string ='';
  ticketPrice: number = 0;
  selectedCategory: string = '';
  selectedLocation: string = '';

  loginEvent: EventEmitter<boolean> = new EventEmitter<boolean>(); 
  roleEvent: EventEmitter<string> = new EventEmitter<string>(); 
  private registerUrl = 'http://localhost:5299/api/Account';
  // private registerUrl = 'https://eventhubfusion.azurewebsites.net/api/Account';

  private eventsUrl = 'http://localhost:5299/api/Event';

  private profileUrl = 'http://localhost:5299/api/Person';

  private getPersonByRoleUrl ='http://localhost:5299/api/Person/getpersonbyrole';

  private createEventUrl = 'http://localhost:5299/api/Event/addEvent';

  private deleteEventUrl = 'http://localhost:5299/api/Event';

  private bookEventUrl = 'http://localhost:5299/api/BookedEvent/BookEvent';

  private eventsBookedByUserUrl = 'http://localhost:5299/api/BookedEvent/GetBookedEventsByUser';

  private eventsBookedByUserIdUrl = 'http://localhost:5299/api/BookedEvent/GetBookedEventsByUserId';

  private getOrganizerCreatedEventUrl = 'http://localhost:5299/api/Event/myevents';

  private getOrganiserEventTicketDetailsUrl = 'http://localhost:5299/api/BookedEvent/tracktickets';

  private updateEventUrl = 'http://localhost:5299/api/Event/updateEvent';
  
  private getTicketDetailsUrl = 'http://localhost:5299/api/Event/eventuserdetails';

  private sendEmailNotificationsUrl = 'http://localhost:5299/api/Email/SendEmailNotification';

  private generateForgotEmailTokenUrl = 'http://localhost:5299/api/ForgetPassword/send-reset-password-token-email';

  private resetPasswordUrl = 'http://localhost:5299/api/ForgetPassword/forget-password';

  private getEventCategoriesUrl = 'http://localhost:5299/api/Event/eventCategories';

  private getEventLocationsUrl = 'http://localhost:5299/api/Event/eventlocation';

  private getEventsByCategoryUrl = 'http://localhost:5299/api/FilterEvents/GetEventsByCategory';

  private getEventsByLocationUrl = 'http://localhost:5299/api/FilterEvents/GetEventsByLocation';

  private getEticketUrl = 'http://localhost:5299/api/SendETickets/generateticket';

  private blockPersonByIdUrl = 'http://localhost:5299/api/Person/blockperson';

  private unBlockPersonByIdUrl = 'http://localhost:5299/api/Person/unblockperson';

  private addReviewUrl = 'http://localhost:5299/events/reviews';

  private getAllReviewUrl ='http://localhost:5299/admin/allreviews';

  private getAllReviewByEventIdUrl ='http://localhost:5299/admin/reviewsbyeventid';

  constructor(private http: HttpClient) { }

  registerUser(userdata: RegisterInterface): Observable<any> {
    return this.http.post(`${this.registerUrl}/register`,userdata);
  }

  loginUser(userdata: LoginInterface): Observable<any> {
    // this.loginEvent.emit(true); 
    return this.http.post(`${this.registerUrl}/login`,userdata);
  }

  logout(): Observable<any> {
    return this.http.post<any>(`${this.registerUrl}/logout`, {});
  }

  generateForgotEmailToken(email: string): Observable<ResponseInterface> {
    return this.http.post<ResponseInterface>(`${this.generateForgotEmailTokenUrl}/${email}`,{});
  }

  resetPassword(resetForm: any): Observable<ResponseInterface> {
    return this.http.post<ResponseInterface>(`${this.resetPasswordUrl}`,resetForm);
  }

  getEvents(): Observable<EventInterface> {
    return this.http.get<EventInterface>(this.eventsUrl);
  }
  getOrganizerEvents(): Observable<OrganizerEventInterface> {
    return this.http.get<OrganizerEventInterface>(this.getOrganizerCreatedEventUrl);
  }

  getOrganizerEventsById(organizerId : string): Observable<OrganizerEventInterface> {
    return this.http.get<OrganizerEventInterface>(`${this.getOrganizerCreatedEventUrl}/${organizerId}`);
  }

  getOrganizerEventTicketDetails(organizerId: string): Observable<EventTicketStatus> {
    return this.http.get<EventTicketStatus>(`${this.getOrganiserEventTicketDetailsUrl}/${organizerId}`);
  }

  editProfile(personId: string, formData:EditProfileInterface): Observable<EditProfileInterface> {
    return this.http.put<EditProfileInterface>(`${this.profileUrl}/updatePerson/${personId}`, formData);
  }
  getProfile(personId: string): Observable<ProfileInterface> {
    return this.http.get<ProfileInterface>(`${this.profileUrl}/${personId}`);
  }

  createEvent(eventdata: CreateEventInterface): Observable<any> {
    return this.http.post(`${this.createEventUrl}`,eventdata);
  }
  updateEvent(eventId: number, eventdata: UpdateEventInterface): Observable<UpdateEventInterface> {
    return this.http.put<UpdateEventInterface>(`${this.updateEventUrl}/${eventId}`, eventdata);
  }

  deleteEvent(eventId:number): Observable<ResponseInterface> {
    return this.http.delete<ResponseInterface>(`${this.deleteEventUrl}/${eventId}`);
  }

  trackTicketDetails(eventId: number): Observable<EventDetailsInterface>{
    return this.http.get<EventDetailsInterface>(`${this.getTicketDetailsUrl}/${eventId}`);
  }

  sendEmailNotifications(eventId: number, emailData: EmailNotificationsInterface): Observable<any> {
    return this.http.post(`${this.sendEmailNotificationsUrl}/${eventId}`,emailData);
  }

  bookEvent(userdata: EventBookingInterface): Observable<any> {
    return this.http.post(`${this.bookEventUrl}`,userdata);
  }

  getUserEvents(): Observable<UserEventsInterface> {
    return this.http.get<UserEventsInterface>(this.eventsBookedByUserUrl);
  }
  getUserEventsId(userId : string): Observable<UserEventsInterface> {
    return this.http.get<UserEventsInterface>(`${this.eventsBookedByUserIdUrl}/${userId}`);
  }
  getEventDetails(eventId : number): Observable<EventDetailsByIdInterface> {
    return this.http.get<EventDetailsByIdInterface>(`${this.eventsUrl}/${eventId}`);
  }
  getPersonByRole(role : string): Observable<GetPersonByRoleInterface> {
    return this.http.get<GetPersonByRoleInterface>(`${this.getPersonByRoleUrl}/${role}`);
  }

  getEventCategories(): Observable<any> {
    return this.http.get<any[]>(this.getEventCategoriesUrl);
  }

  getEventLocations(): Observable<any> {
    return this.http.get<any[]>(this.getEventLocationsUrl);
  }

  getEventsByCategory(selectedCategory: any): Observable<EventByCategoriesInterface> {
    return this.http.get<EventByCategoriesInterface>(`${this.getEventsByCategoryUrl}/${selectedCategory}`);
  }

  getEventsByLocation(selectedLocation: any): Observable<EventByCategoriesInterface> {
    return this.http.get<EventByCategoriesInterface>(`${this.getEventsByLocationUrl}/${selectedLocation}`);
  }

  blockPersonbyId(personId : string): Observable<ResponseInterface> {
    return this.http.post<ResponseInterface>(`${this.blockPersonByIdUrl}/${personId}`,{});
  }
  unBlockPersonbyId(personId : string): Observable<ResponseInterface> {
    return this.http.post<ResponseInterface>(`${this.unBlockPersonByIdUrl}/${personId}`,{});
  }

  getEticket(bookingId: number): Observable<Blob> {
    return this.http.get(`${this.getEticketUrl}/${bookingId}`, {
      responseType: 'blob'
    });
  }

  addReview(userdata: ReviewInterface): Observable<any>{
    return this.http.post(`${this.addReviewUrl}/${this.eventId}`,userdata);
  }

  getAllReviews(): Observable<GetAllReviewsInterface> {
    return this.http.get<GetAllReviewsInterface>(this.getAllReviewUrl);
  }
  getAllReviewsByEventId(eventId : number): Observable<GetAllReviewsInterface> {
    return this.http.get<GetAllReviewsInterface>(`${this.getAllReviewByEventIdUrl}/${eventId}`);
  }

}
