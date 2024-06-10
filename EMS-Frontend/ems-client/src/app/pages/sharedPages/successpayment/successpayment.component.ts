import { Component, OnInit } from '@angular/core';
import { UserdataService } from '../../../services/userDataService/userdata.service';

@Component({
  selector: 'app-successpayment',
  standalone: true,
  imports: [],
  templateUrl: './successpayment.component.html',
  styleUrl: './successpayment.component.css'
})
export class SuccesspaymentComponent implements OnInit{
  
  constructor(private userdataService : UserdataService) {
    
  }

  ngOnInit(): void {
      // this.userdataService.successPayment(){
        
      // }
  }

}
