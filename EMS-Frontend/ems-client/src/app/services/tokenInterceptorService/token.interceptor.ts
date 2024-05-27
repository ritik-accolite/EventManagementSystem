import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';

export const tokenInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  let token: string | null = null;
  
  if (req.url === 'https://eventhubfusion.azurewebsites.net/api/Event') {
    // If the request URL matches the excluded URL, bypass the interceptor
    return next(req);
  }
  if (req.url === 'http://localhost:5299/api/Event') {
    // If the request URL matches the excluded URL, bypass the interceptor
    return next(req);
  }

  token = localStorage?.getItem("jwt");
  const modifiedReq = req.clone({
    headers: req.headers.set('Authorization', `Bearer ${token}`),
  });

  console.log("hitted token interceptor")
  return next(modifiedReq);
};