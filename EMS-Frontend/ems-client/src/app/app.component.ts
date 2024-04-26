import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, RouterOutlet } from '@angular/router';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { HomeComponent } from './pages/home/home.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, HttpClientModule, CommonModule , HomeComponent , RouterModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit{
  title = 'ems-client';
  events: any[] = [];

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.fetchEvents();
  }

  fetchEvents(): void {
    // Replace 'YOUR_API_URL' with the actual API endpoint URL
    this.http.get<any[]>('http://localhost:5299/weatherforecast')
      .subscribe(data => {
        this.events = data;
      });
  }
}

