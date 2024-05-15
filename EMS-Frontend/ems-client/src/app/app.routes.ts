import { Routes } from '@angular/router';
import { HomeComponent } from './pages/sharedPages/home/home.component';
import { MybookingsComponent } from './pages/mybookings/mybookings.component';
import { NeweventComponent } from './pages/organizerPages/newevent/newevent.component';
import { EventlistComponent } from './pages/sharedPages/eventlist/eventlist.component';
import { CommondashComponent } from './pages/sharedPages/commondash/commondash.component';
import { RegisterComponent } from './pages/sharedPages/register/register.component';
import { LoginComponent } from './pages/sharedPages/login/login.component';
import { EventbookingComponent } from './pages/eventbooking/eventbooking.component';
import { NavbarComponent } from './pages/sharedPages/navbar/navbar.component';
import { UserprofileComponent } from './pages/sharedPages/userprofile/userprofile.component';
import { SidebarComponent } from './pages/sharedPages/sidebar/sidebar.component';
import { MyeventsComponent } from './pages/organizerPages/myevents/myevents.component';
import { AuthGuard } from './guards/auth.guard';
import { AboutusComponent } from './pages/sharedPages/aboutus/aboutus.component';
import { ContactusComponent } from './pages/sharedPages/contactus/contactus.component';
import { OrganizerstatComponent } from './pages/organizerPages/organizerstat/organizerstat.component';
import { EditeventComponent } from './pages/organizerPages/editevent/editevent.component';
import { VieweventComponent } from './pages/organizerPages/viewevent/viewevent.component';
import { ForgotpasswordComponent } from './pages/sharedPages/forgotpassword/forgotpassword.component';
import { AdmindashComponent } from './pages/adminPages/admindash/admindash.component';
import { TrackeventComponent } from './pages/adminPages/trackevent/trackevent.component';
import { TrackorgainzersComponent } from './pages/adminPages/trackorgainzers/trackorgainzers.component';
import { ReportedissuesComponent } from './pages/adminPages/reportedissues/reportedissues.component';

export const routes: Routes = [
    {
        path:'',
        redirectTo:'Home',
        pathMatch:'full'
    },
    {
        path:'Home',
        component:HomeComponent,
        children: [
            { path: 'event-list', component: EventlistComponent }
          ]
    },
    {
        path:'new-event',
        component:NeweventComponent,
        canActivate: [AuthGuard]
    },
    {
        path:'event-list',
        component:EventlistComponent
    },
    {
        path: 'admin-dash',
        component : AdmindashComponent,
        children : [
            {path: 'user-profile', component : UserprofileComponent},
            {path: 'track-event', component : TrackeventComponent},
            {path: 'track-organizer', component : TrackorgainzersComponent},
            {path: 'issues', component : ReportedissuesComponent},
            {path: 'app-myevents', component: MyeventsComponent},
        ],
        canActivate : [AuthGuard]
    },
    {
        path:'user-dash',
        component:CommondashComponent,
        children: [
            { path: 'user-profile', component: UserprofileComponent },
            { path:'mybookings', component:MybookingsComponent },
            { path: 'new-event', component: NeweventComponent},
            { path: 'app-myevents', component: MyeventsComponent},
            { path: 'app-viewevent', component : VieweventComponent},
            { path: 'app-editevent', component: EditeventComponent },
            { path:'event-bookings', component:EventbookingComponent},
            { path:'event-list', component:EventlistComponent,
            },
            { path:'event-list',
                children: 
                [
                    { path:'event-bookings', component:EventbookingComponent}
                ]
            },
            { path: '**', component: OrganizerstatComponent }
          ]
        , canActivate: [AuthGuard]
    },
    {
        path:'register',
        component:RegisterComponent
    },
    {
        path:'login',
        component:LoginComponent
    },
    {
        path:'forgot-password',
        component:ForgotpasswordComponent
    },
    {
        path:'navbar',
        component:NavbarComponent
    },
    {
        path:'sidebar',
        component:SidebarComponent,
        canActivate: [AuthGuard]
    },
    {
        path:'my-events',
        component:MyeventsComponent,
        canActivate: [AuthGuard]
    },
    {
        path:'aboutus',
        component:AboutusComponent
    },
    {
        path:'contactus',
        component: ContactusComponent
    }
];

