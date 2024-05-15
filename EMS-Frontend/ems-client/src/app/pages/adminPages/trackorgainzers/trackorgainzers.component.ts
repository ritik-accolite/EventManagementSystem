import { Component } from '@angular/core';
import { UserdataService } from '../../../services/userDataService/userdata.service';
import { NgFor, NgIf } from '@angular/common';
import { Router } from '@angular/router';
import { JwtDecodeService } from '../../../services/jwtDecodeService/jwtDecode.service';
import { response } from 'express';
import { error } from 'console';

@Component({
  selector: 'app-trackorgainzers',
  standalone: true,
  imports: [NgFor, NgIf],
  templateUrl: './trackorgainzers.component.html',
  styleUrl: './trackorgainzers.component.css'
})
export class TrackorgainzersComponent {
  organizers : any [] = [];
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
        (response : any ) => { // change any here !!
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
      this.router.navigate(['user-dash', 'mybookings']);
    }
  }

  blockUser(personId : string){
    this.userdataservice.blockPersonbyId(personId).
    subscribe(
      (response : any) =>{
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
      (response : any) =>{
        this.router.navigate(['admin-dash','track-organizer']);
        console.log('response regarding unblocking',response);
      },
      (error : any) =>{
        console.log('Error regarding Unblocking',error)
      }
    )
  }
}