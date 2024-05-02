import { EventEmitter, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserdataService {

  private registerUrl = 'http://localhost:5299/api/Account';
  loginEvent: EventEmitter<boolean> = new EventEmitter<boolean>();

  constructor(private http: HttpClient) { }

  registerUser(userdata: any): Observable<any> {
    return this.http.post(`${this.registerUrl}/register`,userdata);
  }

  loginUser(userdata: any): Observable<any> {
    this.loginEvent.emit(true); // Emit event upon successful login
    return this.http.post(`${this.registerUrl}/login`,userdata);
  }

  
  logout(): Observable<any> {
    return this.http.post<any>(`${this.registerUrl}/logout`, {});
  }
}
