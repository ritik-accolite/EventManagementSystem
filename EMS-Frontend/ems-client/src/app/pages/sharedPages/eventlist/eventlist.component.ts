import { DatePipe, NgFor, NgIf, CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component , OnInit, CUSTOM_ELEMENTS_SCHEMA, inject } from '@angular/core';
import { UserdataService } from '../../../services/userDataService/userdata.service';
import { ActivatedRoute, Router, RouterLink, RouterOutlet } from '@angular/router';
import { EventInterface } from '../../../interface/commonInterface/event-interface';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-eventlist',
  standalone: true,
  imports: [NgIf,NgFor, RouterLink, DatePipe, CommonModule, RouterOutlet ],
  templateUrl: './eventlist.component.html',
  styleUrls: ['./eventlist.component.css', '../contactus/contactus.component.css'],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class EventlistComponent implements OnInit {
  title = 'ems-client';
  events: any[] = [];
  organizer: any[] = [];
  toaster = inject(ToastrService);

  constructor(
    private http: HttpClient,
    private userdataservice: UserdataService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.fetchEvents();
  }

  fetchEvents(): void {
    this.userdataservice.getEvents().subscribe(
      (response: EventInterface) => {
        this.events = response.allEvents;
      },
      (error) => console.error('Error fetching events: ', error)
    );
  }


  bookEvent(eventId: number, organizerId: string, ticketPrice: number): void {
    if(localStorage.getItem('jwt')!=null){
      this.userdataservice.eventId = eventId;
      this.userdataservice.organizerId = organizerId;
      this.userdataservice.ticketPrice = ticketPrice;
      this.router.navigate(['user-dash','event-bookings']);
    }
    else{
      this.toaster.info('Please Login to book the events!');
      this.router.navigate(['/login']);
    }
  }
  }

