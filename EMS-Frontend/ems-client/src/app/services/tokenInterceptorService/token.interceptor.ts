import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { SpinnerService } from '../spinnerService/spinner.service';
import { finalize } from 'rxjs/operators';


export const tokenInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const spinnerService = inject(SpinnerService);
  let token: string | null = null;

  spinnerService.requestStarted(); // Start the spinner

  if (req.url === 'https://eventfusion.azurewebsites.net/api/Event' || req.url === 'http://localhost:5299/api/Event') {
    return next(req).pipe(
      finalize(() => spinnerService.requestEnded()) // Stop the spinner after request
    );
  }

  token = localStorage?.getItem('jwt');
  const modifiedReq = req.clone({
    headers: req.headers.set('Authorization', `Bearer ${token}`),
  });

  // return next(modifiedReq);
  return next(modifiedReq).pipe(
    finalize(() => spinnerService.requestEnded()) // Stop the spinner after request
  );
};



