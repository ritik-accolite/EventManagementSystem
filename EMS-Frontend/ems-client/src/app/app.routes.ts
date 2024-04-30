import { Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';
import { MybookingsComponent } from './pages/mybookings/mybookings.component';
import { NeweventComponent } from './pages/newevent/newevent.component';
import { EventbokkingsComponent } from './pages/eventbokkings/eventbokkings.component';
import { EventlistComponent } from './pages/eventlist/eventlist.component';

export const routes: Routes = [
    {
        path:'',
        redirectTo:'Home',
        pathMatch:'full'
    },
    {
        path:'Home',
        component:HomeComponent
    },
    {
        path:'mybookings',
        component:MybookingsComponent
    },
    {
        path:'new-event',
        component:NeweventComponent
    },
    {
        path:'event-bookings',
        component:EventbokkingsComponent
    },
    {
        path:'event-list',
        component:EventlistComponent
    }
];

