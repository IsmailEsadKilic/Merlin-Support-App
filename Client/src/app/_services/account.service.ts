import { Injectable } from '@angular/core';
import { User } from '../_models/user';
import { environment } from '../environment';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiUrl + 'account';
  private currentUserSource = new BehaviorSubject<User | null>(null);
  currentUser$ = this.currentUserSource.asObservable();
  
  constructor(private http: HttpClient) { }

  login(login: Login) {
    return this.http.post<User>(this.baseUrl + '/login', login).pipe(
      map((response: User) => {
        const user = response;
        if (user) {
          this.setCurrentUser(user);
        }
      })
    )
  }

  changePassword(changePasswordDto: ChangePasswordDto) {
    return this.http.post<boolean>(this.baseUrl + '/changePassword', changePasswordDto);
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUserSource.next(null);  
  }

  setCurrentUser(user: User) {
    user.roles = [];
    const roles = this.getDecodedToken(user.token).role;
    Array.isArray(roles) ? user.roles = roles : user.roles.push(roles);
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSource.next(user);
  }

  getDecodedToken(token: string) {
    return JSON.parse(atob(token.split('.')[1]));
  }
}



export interface Login {
  userName: string;
  password: string;
}
interface ChangePasswordDto {
  currentPassword: string;
  newPassword: string;
}
