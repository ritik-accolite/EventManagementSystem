import { NgIf } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RouterOutlet,NgIf],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {

  isLoginView: boolean=true;
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

  
  openRegister() {
    this.isLoginView = false;
    const model = document.getElementById('myModal');
    if(model !=null){
      model.style.display = 'block';
    }
  }
  openLogin() {
    const model = document.getElementById('myModal');
    if(model !=null){
      model.style.display = 'block';
    }
  }
  closeModel(){
    const model = document.getElementById('myModal');
    if(model !=null){
      model.style.display = 'none';
    }
  }

  onRegister() {
    this.http.post('',this.registerObj).subscribe((res:any)=>{
      if(res.result){
        alert('Registration Success');
      } else{
        alert(res.message)
      }
    })
  }

  onLogin() {
    this.http.post('',this.loginObj).subscribe((res:any)=>{
      if(res.result){
        alert('Login Success');

        //Login thing integration with database

        this.closeModel()
      } else{
        alert(res.message)
      }
    })
  }

}
