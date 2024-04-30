import { Component, HostListener, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterLinkActive, RouterModule, RouterOutlet } from '@angular/router';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { HomeComponent } from './pages/home/home.component';
import { Router } from 'express';
import { NavbarComponent } from "./pages/navbar/navbar.component";

@Component({
    selector: 'app-root',
    standalone: true,
    templateUrl: './app.component.html',
    styleUrl: './app.component.css',
    providers: [NavbarComponent, HomeComponent],
    imports: [RouterOutlet, HttpClientModule, CommonModule, RouterModule, RouterLink, RouterLinkActive, NavbarComponent]
})
export class AppComponent { //implements OnInit{
  // title = 'ems-client';
  // events: any[] = [];

  // constructor(private http: HttpClient) { }

  // ngOnInit(): void {
  //   this.fetchEvents();
  // }

  // fetchEvents(): void {
  //   // Replace 'YOUR_API_URL' with the actual API endpoint URL
  //   this.http.get<any[]>('http://localhost:5299/weatherforecast')
  //     .subscribe(data => {
  //       this.events = data;
  //     });
  // }

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
  
  // openRegister() {
  //   this.isLoginView = false;
  //   const model = document.getElementById('myModal');
  //   if(model !=null){
  //     model.style.display = 'block';
  //   }
  // }
  // openLogin() {
  //   this.loginPopupVisible = true;
  //   const model = document.getElementById('myModal');
  //   if(model !=null){
  //     model.style.display = 'block';
  //   }
  // }
  // closeModel(){
  //   const model = document.getElementById('myModal');
  //   if(model !=null){
  //     model.style.display = 'none';
  //   }
  // }

  // onRegister() {
  //   this.http.post('',this.registerObj).subscribe((res:any)=>{
  //     if(res.result){
  //       alert('Registration Success');
  //       this.closeModel()
  //     } else{
  //       alert(res.message)
  //     }
  //   })
  // }

  // onLogin() {
  //   this.http.post('',this.loginObj).subscribe((res:any)=>{
  //     if(res.result){
  //       alert('Login Success');

  //       //Login thing integration with database

  //       this.closeModel()
  //     } else{
  //       alert(res.message)
  //     }
  //   })
  // }

}

