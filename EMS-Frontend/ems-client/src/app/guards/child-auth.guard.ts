// child-auth.guard.ts

import { Injectable } from '@angular/core';
import { CanActivateChild, Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
@Injectable({
  providedIn: 'root',
})
export class ChildAuthGuard implements CanActivateChild {
  constructor(private router: Router, private jwtHelper: JwtHelperService) {}

  canActivateChild(): boolean {
    try {
      const token = localStorage?.getItem('jwt');

      if (token && !this.jwtHelper.isTokenExpired(token)) {
        return true;
      }
      this.router.navigate(['/login']);
      return false;
    } catch (e) {
      return false;
    }
  }
}
