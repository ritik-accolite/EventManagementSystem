import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  Router,
  RouterStateSnapshot,
} from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { JwtDecodeService } from '../../services/jwtDecodeService/jwtDecode.service';

@Injectable({
  providedIn: 'root',
})
export class organizerGuard implements CanActivate {
  constructor(
    private router: Router,
    private jwtHelper: JwtHelperService,
    private jwtDecodeService: JwtDecodeService
  ) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    try {
      const token = localStorage?.getItem('jwt');
      const role = localStorage?.getItem('Role');
      if (
        token &&
        !this.jwtHelper.isTokenExpired(token) &&
        role == 'Organizer'
      ) {
        return true;
      }
      this.router.navigate(['/unauthorised']);
      return false;
    } catch (e) {
      return false;
    }
  }
}
