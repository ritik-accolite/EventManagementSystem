import { Component } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-userdash',
  standalone: true,
  imports: [RouterLink , RouterOutlet],
  templateUrl: './userdash.component.html',
  styleUrl: './userdash.component.css'
})
export class UserdashComponent {

}
