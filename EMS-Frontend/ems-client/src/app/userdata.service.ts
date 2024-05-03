import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserdataService {

  private registerUrl = 'http://localhost:5299/api/Account';

  private eventsUrl = 'http://localhost:5299/api/Event';

  private profileUrl = 'http://localhost:5299/api/Person';

  constructor(private http: HttpClient) { }

  registerUser(userdata: any): Observable<any> {
    return this.http.post(`${this.registerUrl}/register`,userdata);
  }

  loginUser(userdata: any): Observable<any> {
    return this.http.post(`${this.registerUrl}/login`,userdata);
  }

  getEvents(): Observable<any[]> {
    return this.http.get<any[]>(this.eventsUrl);
  }

  getProfile(): Observable<any[]> {
    return this.http.get<any[]>(this.profileUrl);
  }
}
