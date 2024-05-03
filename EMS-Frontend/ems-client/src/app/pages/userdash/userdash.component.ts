import { Component } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';
import { SidebarComponent } from '../sidebar/sidebar.component';

@Component({
  selector: 'app-userdash',
  standalone: true,
  imports: [RouterLink , RouterOutlet , SidebarComponent],
  templateUrl: './userdash.component.html',
  styleUrl: './userdash.component.css'
})
export class UserdashComponent {

}
