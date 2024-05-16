import { EventEmitter, Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserdataService {
  eventId : number = 0;
  organizerId: string ='';
  ticketPrice: number = 0;
  selectedCategory: string = '';
  selectedLocation: string = '';

  private registerUrl = 'http://localhost:5299/api/Account';
  loginEvent: EventEmitter<boolean> = new EventEmitter<boolean>();

  private eventsUrl = 'http://localhost:5299/api/Event';

  private profileUrl = 'http://localhost:5299/api/Person';

  private getPersonByRoleUrl ='http://localhost:5299/api/Person/getpersonbyrole';

  private createEventUrl = 'http://localhost:5299/api/Event/addEvent';

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

  private getEventsByCategoryUrl = 'http://localhost:5299/api/FilterEvents/GetEventsByCategory';

  private getEventsByLocationUrl = 'http://localhost:5299/api/FilterEvents/GetEventsByLocation';

  private getEticketUrl = 'http://localhost:5299/api/SendETickets/generateticket';

  private blockPersonByIdUrl = 'http://localhost:5299/api/Person/blockperson';

  private unBlockPersonByIdUrl = 'http://localhost:5299/api/Person/unblockperson';

  private getAllReviewUrl ='http://localhost:5299/admin/allreviews';

  private getAllReviewByEventIdUrl ='http://localhost:5299/reviews';

  constructor(private http: HttpClient) { }

  registerUser(userdata: any): Observable<any> {
    return this.http.post(`${this.registerUrl}/register`,userdata);
  }

  loginUser(userdata: any): Observable<any> {
    // this.loginEvent.emit(true); 
    return this.http.post(`${this.registerUrl}/login`,userdata);
  }

  logout(): Observable<any> {
    return this.http.post<any>(`${this.registerUrl}/logout`, {});
  }

  generateForgotEmailToken(email: string): Observable<any> {
    return this.http.post<any>(`${this.generateForgotEmailTokenUrl}/${email}`,{});
  }

  resetPassword(resetForm: any): Observable<any> {
    return this.http.post<any>(`${this.resetPasswordUrl}`,resetForm);
  }

  getEvents(): Observable<any[]> {
    return this.http.get<any[]>(this.eventsUrl);
  }
  getOrganizerEvents(): Observable<any[]> {
    return this.http.get<any[]>(this.getOrganizerCreatedEventUrl);
  }

  getOrganizerEventsById(organizerId : string): Observable<any[]> {
    return this.http.get<any[]>(`${this.getOrganizerCreatedEventUrl}/${organizerId}`);
  }

  getOrganizerEventTicketDetails(organizerId: string): Observable<any[]> {
    return this.http.get<any[]>(`${this.getOrganiserEventTicketDetailsUrl}/${organizerId}`);
  }

  editProfile(personId: string, formData:any): Observable<any> {
    return this.http.put(`${this.profileUrl}/updatePerson/${personId}`, formData);
  }
  getProfile(personId: string): Observable<any[]> {
    return this.http.get<any>(`${this.profileUrl}/${personId}`);
  }

  createEvent(eventdata: any): Observable<any> {
    return this.http.post(`${this.createEventUrl}`,eventdata);
  }
  updateEvent(eventId: number, eventdata: any): Observable<any> {
    return this.http.put(`${this.updateEventUrl}/${eventId}`, eventdata);
  }

  deleteEvent(eventId:number): Observable<any> {
    return this.http.delete(`${this.createEventUrl}/${eventId}`);
  }

  trackTicketDetails(eventId: number): Observable<any>{
    return this.http.get(`${this.getTicketDetailsUrl}/${eventId}`);
  }

  sendEmailNotifications(eventId: number, emailData: any): Observable<any> {
    return this.http.post(`${this.sendEmailNotificationsUrl}/${eventId}`,emailData);
  }

  bookEvent(userdata: any): Observable<any> {
    return this.http.post(`${this.bookEventUrl}`,userdata);
  }

  getUserEvents(): Observable<any> {
    return this.http.get<any[]>(this.eventsBookedByUserUrl);
  }
  getUserEventsId(userId : string): Observable<any> {
    return this.http.get<any[]>(`${this.eventsBookedByUserIdUrl}/${userId}`);
  }
  getEventDetails(eventId : number): Observable<any> {
    return this.http.get<any[]>(`${this.eventsUrl}/${eventId}`);
  }
  getPersonByRole(role : string): Observable<any> {
    return this.http.get<any[]>(`${this.getPersonByRoleUrl}/${role}`);
  }

  getEventCategories(): Observable<any> {
    return this.http.get<any[]>(this.getEventCategoriesUrl);
  }

  getEventsByCategory(selectedCategory: any): Observable<any> {
    return this.http.get<any[]>(`${this.getEventsByCategoryUrl}/${selectedCategory}`);
  }

  getEventsByLocation(selectedLocation: any): Observable<any> {
    return this.http.get<any[]>(`${this.getEventsByLocationUrl}/${selectedLocation}`);
  }

  blockPersonbyId(personId : string): Observable<any> {
    return this.http.post<any[]>(`${this.blockPersonByIdUrl}/${personId}`,{});
  }
  unBlockPersonbyId(personId : string): Observable<any> {
    return this.http.post<any[]>(`${this.unBlockPersonByIdUrl}/${personId}`,{});
  }

  getEticket(bookingId: number): Observable<Blob> {
    return this.http.get(`${this.getEticketUrl}/${bookingId}`, {
      responseType: 'blob'
    });
  }

  getAllReviews(): Observable<any> {
    return this.http.get<any>(this.getAllReviewUrl);
  }
  getAllReviewsByEventId(eventId : number): Observable<any> {
    return this.http.get<any>(`${this.getAllReviewByEventIdUrl}/${eventId}`);
  }

}
