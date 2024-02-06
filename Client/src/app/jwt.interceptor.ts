import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, take } from 'rxjs';
import { AccountService } from './_services/account.service';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  constructor(private accountService: AccountService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => {
        // console.log("user: ", user);
        if (user) {
          // console.log("service user: ", user);
          request = request.clone({
            setHeaders: {
              Authorization: `Bearer ${user.token}`
            }
          })
        } else {
          // console.log("local user: ", localStorage.getItem('user'));
          try {
            const user = JSON.parse(localStorage.getItem('user')!);
            request = request.clone({
              setHeaders: {
                Authorization: `Bearer ${user.token}`
              }
            })
          } catch (error) {
            console.log(error);
          }
        }
      }
    })
    // console.log("request: ", request);
    return next.handle(request);
  }
}
