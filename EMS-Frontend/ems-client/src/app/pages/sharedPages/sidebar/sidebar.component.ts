import { Component, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { NeweventComponent } from '../../organizerPages/newevent/newevent.component';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [NeweventComponent, RouterLink, CommonModule, FormsModule],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css',
})
export class SidebarComponent {
  role: any;
  toggleBar = false;

  constructor(private router: Router) {}

  ngOnInit(): void {
    this.role = localStorage.getItem('Role');
    if (this.role === null) {
      this.router.navigate(['/login']);
    }
  }

  deactivateToggle(): void {
    this.toggleBar = true;
    const doc = document.getElementById(
      'navbarSupportedSideContent'
    ) as HTMLElement;
    doc.style.display = 'none';
  }

  activateToggle(): void {
    const doc = document.getElementById(
      'navbarSupportedSideContent'
    ) as HTMLElement;
    if (this.toggleBar === true) {
      doc.style.display = 'block';
      this.toggleBar = false;
    }
  }
  toggleSidebar() {
    const sidebarCollapse = document.getElementById('navbarSupportedSideContent');
    const sidebarToggler = document.querySelector('.navbar-toggler');
    
    if (sidebarCollapse && sidebarToggler) {
      sidebarToggler.classList.add('collapsed');
      sidebarCollapse.classList.remove('show');
    }
  }
  onTabClick(tabRoute: string) {
    if (tabRoute === 'organizer-profile') {
      this.router.navigate(['organizer-dash', 'user-profile']);
    } else if (tabRoute === 'user-profile') {
      this.router.navigate(['user-dash', 'user-profile']);
    } else if (tabRoute === 'myevents') {
      this.router.navigate(['organizer-dash', 'app-myevents']);
    } else if (tabRoute === 'new-event') {
      this.router.navigate(['organizer-dash', 'new-event']);
    } else if (tabRoute === 'event-list') {
      this.router.navigate(['user-dash', 'event-list']);
    } else if (tabRoute === 'mybookings') {
      this.router.navigate(['user-dash', 'mybookings']);
    } else if (tabRoute === 'user-stat') {
      this.router.navigate(['user-dash', '']);
    } else if (tabRoute === 'organizer-stat') {
      this.router.navigate(['organizer-dash', 'app-organizerstat']);
    } else if (tabRoute === 'organizer-profile') {
      this.router.navigate(['organizer-dash', 'user-profile']);
    } else if (tabRoute === 'track-event') {
      this.router.navigate(['admin-dash', 'track-event']);
    } else if (tabRoute === 'track-organizer') {
      this.router.navigate(['admin-dash', 'track-organizer']);
    } else if (tabRoute === 'track-user') {
      this.router.navigate(['admin-dash', 'track-organizer']);
    } else if (tabRoute === 'issues') {
      this.router.navigate(['admin-dash', 'issues']);
    } else if (tabRoute === 'admin-profile') {
      this.router.navigate(['admin-dash', 'user-profile']);
    } else if (tabRoute === 'user-stat') {
      this.router.navigate(['user-dash', 'user-stat']);
    }
  }
}
