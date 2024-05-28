import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { AppComponent } from './app/app.component';
import { register as registerSwiperModule } from 'swiper/element/bundle';


registerSwiperModule();
bootstrapApplication(AppComponent, appConfig)
  .catch((err) => console.error(err));
