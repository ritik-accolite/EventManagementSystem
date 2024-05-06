import { Component } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { NeweventComponent } from '../newevent/newevent.component';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [NeweventComponent, RouterLink],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css'
})
export class SidebarComponent {
  constructor(private router: Router) {}

  onTabClick(tabRoute: string) {
    if (tabRoute === 'user-profile') {
      this.router.navigate(['user-dash', 'user-profile']);
    } else if (tabRoute === 'mybookings') {
      this.router.navigate(['user-dash','mybookings']);
    }
  }
}
