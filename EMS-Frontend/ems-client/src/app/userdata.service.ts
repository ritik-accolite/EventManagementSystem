import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserdataService {

  private registerUrl = 'http://localhost:5299/api/Account';

  constructor(private http: HttpClient) { }

  registerUser(userdata: any): Observable<any> {
    return this.http.post(`${this.registerUrl}/register`,userdata);
  }

  loginUser(userdata: any): Observable<any> {
    return this.http.post(`${this.registerUrl}/login`,userdata);
  }
}
