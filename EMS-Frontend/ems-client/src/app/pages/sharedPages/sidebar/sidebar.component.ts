import { Component, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { NeweventComponent } from '../../organizerPages/newevent/newevent.component';
import { JwtDecodeService } from '../../../services/jwtDecodeService/jwtDecode.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [NeweventComponent, RouterLink, CommonModule],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css'
})
export class SidebarComponent {
  role : string = '';
  constructor(private router: Router,
              private jwtDecodeService : JwtDecodeService
  ) {}

  ngOnInit(): void{
    this.role = this.jwtDecodeService.role;
    if (this.role === null) {
      this.router.navigate(['/login']);
    }
  }

  onTabClick(tabRoute: string) {
    if (tabRoute === 'user-profile') {
      this.router.navigate(['user-dash', 'user-profile']);
    } else if (tabRoute === 'myevents') {
      this.router.navigate(['user-dash','app-myevents']);
    } else if (tabRoute === 'new-event')  {
      this.router.navigate(['user-dash', 'new-event'])
    } else if (tabRoute === 'event-list') {
      this.router.navigate(['user-dash','event-list'])
    } else if (tabRoute === 'mybookings') {
      this.router.navigate(['user-dash','mybookings'])
    } else if (tabRoute === 'organizer-stat') {
      this.router.navigate(['user-dash','app-organizerstat'])
    } else if (tabRoute === 'organizer-profile') {
      this.router.navigate(['admin-dash','user-profile'])
    } else if (tabRoute === 'track-event') {
      this.router.navigate(['admin-dash','track-event'])
    } else if (tabRoute === 'track-organizer') {
      this.router.navigate(['admin-dash','track-organizer'])
    } else if (tabRoute === 'track-user') {
      this.router.navigate(['admin-dash','track-organizer'])
    } else if (tabRoute === 'issues') {
      this.router.navigate(['admin-dash','issues'])
    }
  }
}
