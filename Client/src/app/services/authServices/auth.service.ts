import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from '../../interfaces/user';
import { environment } from '../../../../environments/environment.development';
import { BaseService } from '../BaseService/base.service';

@Injectable({
  providedIn: 'root',
})
export class AuthService extends BaseService {

  login(userData: User): Observable<any> {
    return this.http.post(`${environment.BASE_API_ENDPOINT}/auth/login`, userData);
  }

  register(userData: User): Observable<any> {
    return this.http.post(`${environment.BASE_API_ENDPOINT}/auth/register`, userData);
  }

  getTokenByrefreshToken(refreshToken: string): Observable<any> {
    return this.http.get(`${environment.BASE_API_ENDPOINT}/token/${refreshToken}`);
  }
}