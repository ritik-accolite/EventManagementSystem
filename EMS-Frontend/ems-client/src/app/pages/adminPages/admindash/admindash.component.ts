import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { SidebarComponent } from '../../sharedPages/sidebar/sidebar.component';

@Component({
  selector: 'app-admindash',
  standalone: true,
  imports: [RouterOutlet, SidebarComponent],
  templateUrl: './admindash.component.html',
  styleUrl: './admindash.component.css'
})
export class AdmindashComponent {

}
