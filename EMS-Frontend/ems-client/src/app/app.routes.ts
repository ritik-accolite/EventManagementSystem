import { Routes } from '@angular/router';
import { HomeComponent } from './pages/sharedPages/home/home.component';
import { MybookingsComponent } from './pages/userPages/mybookings/mybookings.component';
import { NeweventComponent } from './pages/organizerPages/newevent/newevent.component';
import { EventlistComponent } from './pages/sharedPages/eventlist/eventlist.component';
import { CommondashComponent } from './pages/sharedPages/commondash/commondash.component';
import { RegisterComponent } from './pages/sharedPages/register/register.component';
import { LoginComponent } from './pages/sharedPages/login/login.component';
import { EventbookingComponent } from './pages/userPages/eventbooking/eventbooking.component';
import { NavbarComponent } from './pages/sharedPages/navbar/navbar.component';
import { UserprofileComponent } from './pages/sharedPages/userprofile/userprofile.component';
import { SidebarComponent } from './pages/sharedPages/sidebar/sidebar.component';
import { MyeventsComponent } from './pages/organizerPages/myevents/myevents.component';
import { AuthGuard } from './guards/auth.guard';
import { ChildAuthGuard } from './guards/child-auth.guard';
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
import { EventbycategoryComponent } from './pages/userPages/eventbycategory/eventbycategory.component';
import { EventbylocationComponent } from './pages/userPages/eventbylocation/eventbylocation.component';
import { ReviewComponent } from './pages/userPages/review/review.component';
import { EventreviewComponent } from './pages/organizerPages/eventreview/eventreview.component';
import { UserstatComponent } from './pages/userPages/userstat/userstat.component';
import { adminGuard } from './guards/adminGuard/admin.guard';
import { UnauthorisedComponent } from './pages/sharedPages/unauthorised/unauthorised.component';
import { organizerGuard } from './guards/organizerGuard/organizer.guard';
import { userGuard } from './guards/userGuard/user.guard';

export const routes: Routes = [
    {
        path:'event-list',
        component:EventlistComponent
    },
    {
        path:'Home',
        component: HomeComponent,
        children: [
            { path: 'event-list', component: EventlistComponent }
          ]
    },
    {
        path:'event-list',
        component:EventlistComponent
    },
    {
        path: 'admin-dash',
        component : AdmindashComponent,
        canActivate : [adminGuard],
        children : [
            {path: 'user-profile', component : UserprofileComponent},
            {path: 'track-organizer', component : TrackorgainzersComponent},
            {path: 'issues', component : ReportedissuesComponent},
            {path: 'app-myevents', component: MyeventsComponent},
            {path: 'app-viewevent', component : VieweventComponent},
            {path: 'app-eventreview', component : EventreviewComponent},
            {path: '**', component : TrackeventComponent},
        ]
    },
    {
        path : 'organizer-dash',
        component:CommondashComponent,
        canActivate: [organizerGuard],
        children: [
            { path: 'user-profile', component: UserprofileComponent },
            { path: 'new-event', component: NeweventComponent},
            { path: 'app-myevents', component: MyeventsComponent},
            { path: 'app-viewevent', component : VieweventComponent},
            { path: 'app-editevent', component: EditeventComponent },
            { path: 'app-eventreview', component : EventreviewComponent},
            { path:'mybookings', component:MybookingsComponent },
            { path:'event-bookings', component:EventbookingComponent},
            { path: '**', component: OrganizerstatComponent }
        ]
    },
    {
        path:'user-dash',
        component:CommondashComponent,
        canActivate: [userGuard],
        children: [
            { path: 'user-profile', component: UserprofileComponent },
            { path: 'user-stat', component: UserstatComponent },
            { path: 'user-stat', 
                children:
                [
                    { path:'event-by-category', component: EventbycategoryComponent },
                    { path:'event-by-location', component: EventbylocationComponent }
                ]
            },
            { path:'mybookings', component:MybookingsComponent },
            { path:'mybookings', 
                children:
                [
                    { path:'review', component: ReviewComponent}
                ]
            },
            { path:'event-bookings', component:EventbookingComponent},
            // { path:'event-list', component:EventlistComponent },
            // { path:'event-list',
            //     children: 
            //     [
            //         { path:'event-bookings', component:EventbookingComponent}
            //     ]
            // },
            { path: 'event-by-category', component:EventbycategoryComponent},
            { path: 'event-by-location', component:EventbylocationComponent},
            { path: '**', component : UserstatComponent}
          ]
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
        path:'aboutus',
        component:AboutusComponent
    },
    {
        path:'contactus',
        component: ContactusComponent
    },
    {
        path: 'unauthorised',
        component : UnauthorisedComponent
    },
    {
        path:'',
        redirectTo:'Home',
        pathMatch:'full'
    }
];

