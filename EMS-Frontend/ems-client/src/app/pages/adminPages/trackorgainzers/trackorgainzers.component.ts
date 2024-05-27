import { Component, inject } from '@angular/core';
import { UserdataService } from '../../../services/userDataService/userdata.service';
import { NgFor, NgIf } from '@angular/common';
import { Router } from '@angular/router';
import { JwtDecodeService } from '../../../services/jwtDecodeService/jwtDecode.service';
import { response } from 'express';
import { error } from 'console';
import { GetPersonByRoleInterface } from '../../../interface/adminInterface/get-person-by-role-interface';
import { GetAllPersonsByAdminInterface } from '../../../interface/adminInterface/get-all-persons-by-admin-interface';
import { ResponseInterface } from '../../../interface/commonInterface/response-interface';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-trackorgainzers',
  standalone: true,
  imports: [NgFor, NgIf],
  templateUrl: './trackorgainzers.component.html',
  styleUrl: './trackorgainzers.component.css'
})
export class TrackorgainzersComponent {
  organizers : GetAllPersonsByAdminInterface [] = [];
  toaster=inject(ToastrService);
  listTitle: string = "All Organizers";
  constructor(private userdataservice : UserdataService,
              private router : Router,
              private jwtDecodeService : JwtDecodeService
  ) {}

  ngOnInit(): void {
    this.fetchPersons('Organizer'); 
  }

  fetchPersons(role : string): void {
    this.userdataservice.getPersonByRole(role)
      .subscribe(
        (response : GetPersonByRoleInterface ) => { // change any here !!
          this.organizers = response.allPersons; 
          this.listTitle = role;
          console.log('organizers fetched successfully:', this.organizers);
        },
        error => console.error('Error fetching organizers: ', error)
      );
  }

  redirectToViewEvent(personId: string, role: string) {
    if (role === 'Organizer') {
      this.jwtDecodeService.organizerId = personId;
      this.router.navigate(['admin-dash', 'app-myevents']);
    } else if (role === 'User') {
      this.jwtDecodeService.userId = personId;
      this.router.navigate(['admin-dash', 'mybookings']);
    }
  }

  blockUser(personId : string){
    this.userdataservice.blockPersonbyId(personId).
    subscribe(
      (response : ResponseInterface) =>{
        this.toaster.success("Blocked Succesfully");
        this.router.navigate(['admin-dash','track-organizer']);
        console.log('response regarding blocking',response);
      },
      (error : any) =>{
        console.log('Error regarding blocking',error)
      }
    )
  }

  unBlockUser(personId : string){
    this.userdataservice.unBlockPersonbyId(personId).
    subscribe(
      (response : ResponseInterface) =>{
        this.toaster.success("Unblocked Succesfully");
        this.router.navigate(['admin-dash','track-organizer']);
        console.log('response regarding unblocking',response);
      },
      (error : any) =>{
        console.log('Error regarding Unblocking',error)
      }
    )
  }
}
