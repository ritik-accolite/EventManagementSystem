import { Component, OnInit, inject } from '@angular/core';
import { UserdataService } from '../../../services/userDataService/userdata.service';
import { NgFor, NgIf, CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { JwtDecodeService } from '../../../services/jwtDecodeService/jwtDecode.service';
import { GetPersonByRoleInterface } from '../../../interface/adminInterface/get-person-by-role-interface';
import { GetAllPersonsByAdminInterface } from '../../../interface/adminInterface/get-all-persons-by-admin-interface';
import { ResponseInterface } from '../../../interface/commonInterface/response-interface';
import { ToastrService } from 'ngx-toastr';
import { FormBuilder, FormControl, ReactiveFormsModule } from '@angular/forms';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-trackorgainzers',
  standalone: true,
  imports: [NgFor, NgIf, CommonModule, ReactiveFormsModule],
  templateUrl: './trackorgainzers.component.html',
  styleUrls: ['./trackorgainzers.component.css'],
})
export class TrackorgainzersComponent implements OnInit {
  organizers: GetAllPersonsByAdminInterface[] = [];
  filteredOrganizers: GetAllPersonsByAdminInterface[] = [];
  toaster = inject(ToastrService);
  listTitle: string = 'All Organizers';
  searchControl = new FormControl('');

  constructor(
    private fb: FormBuilder,
    private userdataservice: UserdataService,
    private router: Router,
    private jwtDecodeService: JwtDecodeService
  ) {}

  ngOnInit(): void {
    this.fetchPersons('Organizer');
    this.searchControl.valueChanges.subscribe((searchTerm) => {
      this.applyFilter(searchTerm);
    });
  }

  fetchPersons(role: string): void {
    this.userdataservice.getPersonByRole(role).subscribe(
      (response: GetPersonByRoleInterface) => {
        this.organizers = response.allPersons;
        this.filteredOrganizers = this.organizers;
        this.listTitle = role;
      },
      (error) => console.error('Error fetching organizers: ', error)
    );
  }

  applyFilter(searchTerm: any): void {
    const lowerCaseSearchTerm = searchTerm.toLowerCase();
    this.filteredOrganizers = this.organizers.filter((person) =>
      `${person.firstName} ${person.lastName}`.toLowerCase().includes(lowerCaseSearchTerm) ||
      person.email.toLowerCase().includes(lowerCaseSearchTerm)
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

  confirmBlockUser(person: GetAllPersonsByAdminInterface) {
    Swal.fire({
      title: `Block ${person.firstName} ${person.lastName}?`,
      html: `
        <p>Blocking this person will restrict their access to the system.</p>
        <p>Please provide a reason for blocking:</p>
        <input type="text" id="blockReason" class="swal2-input" placeholder="Reason for blocking">
        <p>Email: <strong>${person.email}</strong></p>
        <p>The reason will be sent to this persons email.</p>
      `,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Yes, block them!',
      cancelButtonText: 'No, cancel',
      preConfirm: () => {
        const blockReason = (document.getElementById('blockReason') as HTMLInputElement).value;
        if (!blockReason) {
          Swal.showValidationMessage('Reason is required');
          return false;
        }
        return { blockReason };
      }
    }).then((result) => {
      if (result.isConfirmed) {
        const reason = result.value?.blockReason;
        this.blockUser(person.id);
      }
    });
  }
  
  blockUser(personId: string) {
    // Here you would send the reason along with the request to block the user
    this.userdataservice.blockPersonbyId(personId).subscribe(
      (response: ResponseInterface) => {
        this.toaster.success('Blocked Successfully');
        this.fetchPersons(this.listTitle); // Fetch updated list after blocking
      },
      (error: any) => {
        console.log('Error regarding blocking', error);
      }
    );
  }


  confirmUnblockUser(person: GetAllPersonsByAdminInterface) {
    Swal.fire({
      title: `Unblock ${person.firstName} ${person.lastName}?`,
      text: "This will restore their access to the system.",
      icon: 'info',
      showCancelButton: true,
      confirmButtonText: 'Yes, unblock them!',
      cancelButtonText: 'No, cancel'
    }).then((result) => {
      if (result.isConfirmed) {
        this.unBlockUser(person.id);
      }
    });
  }
  
  unBlockUser(personId: string) {
    this.userdataservice.unBlockPersonbyId(personId).subscribe(
      (response: ResponseInterface) => {
        this.toaster.success('Unblocked Successfully');
        this.fetchPersons(this.listTitle); // Fetch updated list after unblocking
      },
      (error: any) => {
        console.log('Error regarding Unblocking', error);
      }
    );
  }
  
}
