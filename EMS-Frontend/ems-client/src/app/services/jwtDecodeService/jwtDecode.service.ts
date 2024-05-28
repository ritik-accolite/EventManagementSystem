import { Injectable } from '@angular/core';
import { jwtDecode } from 'jwt-decode';

interface DecodedToken {
  [key: string]: string | number | undefined;
  Role: string;
  Id: string;
  exp: number;
  iss: string;
  aud: string;
}

@Injectable({
  providedIn: 'root',
})
export class JwtDecodeService {
  id: string = '';
  role: string = '';
  organizerId: string = '';
  userId: string = '';
  constructor() {}

  decodeToken(token: string): DecodedToken | null {
    try {
      const decodedToken = jwtDecode(token) as DecodedToken;
      this.role = decodedToken['Role'];
      this.id = decodedToken['Id'];
      localStorage.setItem('Role', `${this.role}`);
      localStorage.setItem('LoginUserId', `${this.id}`);
      return decodedToken;
    } catch (error) {
      console.error('Error decoding token:', error);
      return null;
    }
  }
}
