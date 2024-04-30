import { Component } from '@angular/core';
import { UserdataService } from '../../userdata.service';
import { ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {

  userdata = {
    firstname: '',
    lastname:'',
    email: '',
    password: '',
    phonenumber: '',
    role:''
  }

  constructor(private userdataservice: UserdataService){}

  onSubmit(){
    this.userdataservice.registerUser(this.userdata).subscribe(response => {
      console.log('User registered successfully:', response);
    },
    error => {
      console.error('Error registering user:', error);
    }
  )
  }

}
