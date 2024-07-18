import {  HttpErrorResponse,HttpInterceptorFn,} from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import {  catchError,finalize,switchMap,} from 'rxjs';
import { SpinnerService } from '../services/SpinnerServices/spinner.service';
import { AuthService } from '../services/authServices/auth.service';

export const Interceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const spinnerServices = inject(SpinnerService);
  const authService = inject(AuthService);
  spinnerServices.showSpinner.next(true);
  const authreq = req.clone({
    headers: req.headers.set(
      'Authorization',
      'Bearer ' + localStorage.getItem('accessToken')
    ),
  });

  return next(authreq).pipe(
    catchError((err) => {
      let refreshToken = localStorage.getItem('refreshToken')!;
      if ( err instanceof HttpErrorResponse && err.status == 401 && refreshToken != null && refreshToken != undefined) {
        localStorage.clear();
        return authService.getTokenByrefreshToken(refreshToken).pipe(
          switchMap((res) => {
            localStorage.setItem('accessToken', res.jwttoken);
            localStorage.setItem('refreshToken', res.refreshToken);
            const newAuthReq = authreq.clone({
              headers: req.headers.set(
                'Authorization',
                'Bearer ' + localStorage.getItem('accessToken')
              ),
            });
            return next(newAuthReq);
          }),
          catchError((err) => {
            localStorage.clear();
            router.navigateByUrl('/home');
            throw err;
          })
        );
      }
      throw err;
    }),
    finalize(() => {
      spinnerServices.showSpinner.next(false);
    })
  );
};