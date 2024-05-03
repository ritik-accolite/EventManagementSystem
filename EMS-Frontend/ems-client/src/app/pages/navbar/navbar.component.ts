import { Component } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';
import { AngularFontAwesomeComponent } from 'angular-font-awesome';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [RouterLink ],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent {
  searchQuery: string = '';

  constructor() { }

  search() {
    // Perform search action with the searchQuery value
    console.log('Search query:', this.searchQuery);
    // You can call a service method here to perform the actual search
  }

}
