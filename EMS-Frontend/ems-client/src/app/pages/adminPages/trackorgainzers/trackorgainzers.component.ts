import { Component } from '@angular/core';
import { UserdataService } from '../../../services/userDataService/userdata.service';
import { NgFor, NgIf } from '@angular/common';

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
  constructor(private userdataservice : UserdataService) {}

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
}
