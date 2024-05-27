import { ApplicationConfig, importProvidersFrom} from '@angular/core';
import { provideRouter } from '@angular/router';
import { JwtModule } from "@auth0/angular-jwt";
import { routes } from './app.routes';
import { provideClientHydration } from '@angular/platform-browser';
import { provideHttpClient, withFetch, withInterceptors } from '@angular/common/http';
import { tokenInterceptor } from './services/tokenInterceptorService/token.interceptor';
import { provideToastr } from 'ngx-toastr';

export function tokenGetter() { 
  return localStorage?.getItem("jwt"); 
}

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideToastr(), 
    provideHttpClient(withInterceptors([tokenInterceptor])), 
    provideClientHydration(), 
    provideHttpClient(withFetch()),

    importProvidersFrom(JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        allowedDomains: ["localhost:5299"],
        disallowedRoutes: []
      }
    }))
  ]
};
