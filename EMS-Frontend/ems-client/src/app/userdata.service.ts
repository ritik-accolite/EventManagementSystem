import { EventEmitter, Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserdataService {
  eventId : number = 0;

  eventId: number = 0;
  organizerId: string ='';
  ticketPrice: number = 0;

  private registerUrl = 'http://localhost:5299/api/Account';
  loginEvent: EventEmitter<boolean> = new EventEmitter<boolean>();

  private eventsUrl = 'http://localhost:5299/api/Event';

  private profileUrl = 'http://localhost:5299/api/Person';

  private createEventUrl = 'http://localhost:5299/api/Event/addEvent';

  private bookEventUrl = 'http://localhost:5299/api/BookedEvent/BookEvent';

  private eventsBookedByUserUrl = 'http://localhost:5299/api/BookedEvent/GetBookedEventsByUser';

  private getOrganizerCreatedEventUrl = 'http://localhost:5299/api/Event/myevents';

  private getOrganiserEventTicketDetailsUrl = 'http://localhost:5299/api/BookedEvent/tracktickets';

  private updateEventUrl = 'http://localhost:5299/api/Event/updateEvent';
  
  private getTicketDetailsUrl = 'http://localhost:5299/api/Event/eventuserdetails';

  private sendEmailNotificationsUrl = 'http://localhost:5299/api/Email/SendEmailNotification';

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

  getEvents(): Observable<any[]> {
    return this.http.get<any[]>(this.eventsUrl);
  }
  getOrganizerEvents(): Observable<any[]> {
    return this.http.get<any[]>(this.getOrganizerCreatedEventUrl);
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
  getEventDetails(eventId : number): Observable<any> {
    return this.http.get<any[]>(`${this.eventsUrl}/${eventId}`);
  }
}
