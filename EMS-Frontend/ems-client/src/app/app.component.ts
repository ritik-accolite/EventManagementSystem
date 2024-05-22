import { Component, HostListener, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink, RouterLinkActive, RouterModule, RouterOutlet } from '@angular/router';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { HomeComponent } from './pages/sharedPages/home/home.component';
import { Router } from 'express';
import { NavbarComponent } from "./pages/sharedPages/navbar/navbar.component";
import { FooterComponent } from './pages/sharedPages/footer/footer.component';

@Component({
    selector: 'app-root',
    standalone: true,
    templateUrl: './app.component.html',
    styleUrl: './app.component.css',
    providers: [NavbarComponent, HomeComponent, FooterComponent],
    imports: [RouterOutlet, FormsModule, HttpClientModule, CommonModule,
        RouterModule, RouterLink, RouterLinkActive, NavbarComponent,
        FooterComponent]
})
export class AppComponent { 
  registerObj: any = {
    "Id":0,
    "Name":"",
    "PhoneNumber":"",
    "Email":"",
    "Password":"",
    "Role":""
  };

  loginObj : any ={
    "Username":"",
    "Password":""
  };

  constructor(private http: HttpClient){}
}

