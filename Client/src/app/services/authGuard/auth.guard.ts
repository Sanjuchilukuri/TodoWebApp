import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const authGuard: CanActivateFn = () => {
  const router = inject(Router);
  const token = localStorage.getItem('accessToken');
  if (token != null && token != undefined) {
    return true;
  } else {
    localStorage.clear();
    router.navigateByUrl('/home');
    return false;
  }
};
