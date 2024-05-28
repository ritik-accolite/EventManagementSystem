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
export class adminGuard implements CanActivate {
  constructor(
    private router: Router,
    private jwtHelper: JwtHelperService,
    private jwtDecodeService: JwtDecodeService
  ) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    try {
      const token = localStorage?.getItem('jwt');
      const role = this.jwtDecodeService.role;
      if (token && !this.jwtHelper.isTokenExpired(token) && role === 'Admin') {
        return true;
      }
      this.router.navigate(['/unauthorised']);
      console.log('not admin');
      return false;
    } catch (e) {
      console.log('error Admin guard', e);
      return false;
    }
  }
}
