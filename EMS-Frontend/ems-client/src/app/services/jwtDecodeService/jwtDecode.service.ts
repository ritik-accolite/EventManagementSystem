import { Injectable } from '@angular/core';
import  {jwtDecode} from 'jwt-decode';


interface DecodedToken {
  [key: string]: string | number | undefined; // Define index signature
  Role: string; // Define the 'Role' property
  Id: string; // Define the 'Id' property
  exp: number; // Define the 'exp' property
  iss: string; // Define the 'iss' property
  aud: string; // Define the 'aud' property
}

@Injectable({
  providedIn: 'root'
})
export class JwtDecodeService {

  id : string = '';
  role : string = '';
  organizerId : string = '';
  userId : string = '';
  constructor() { }
  
  decodeToken(token: string): DecodedToken | null {
    try {
      console.log(token);
      const decodedToken = jwtDecode(token) as DecodedToken;
      this.role = decodedToken['Role'];
      this.id = decodedToken['Id'];
      localStorage.setItem('Role',`${this.role}`);
      localStorage.setItem('LoginUserId',`${this.id}`);
      return decodedToken;
    } catch (error) {
      console.error('Error decoding token:', error);
      return null;
    }
  }
}
