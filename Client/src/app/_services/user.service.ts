import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../environment';
import { User, UserAdd } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  baseUrl = environment.apiUrl + 'user';
  permDictCache: {[key: number]: string} = {}; // {101: "create user", 102: "create customer", ...}

  constructor(private http: HttpClient) {
    this.getPermDict().subscribe(permissions => {
      this.permDictCache = Object.fromEntries(Object.entries(permissions));
    });
  }

  getUsers() {
    return this.http.get<User[]>(this.baseUrl);
  }

  getUser(id: number) {
    return this.http.get<User>(this.baseUrl + '/' + id);
  }
  
  addUser(userAdd: UserAdd) {
    return this.http.put<User>(this.baseUrl + '/add', userAdd);
  }

  updateUser(userAdd: UserAdd, userId: number) {
    return this.http.post<User>(this.baseUrl + '/update/' + userId, userAdd);
  }

  deleteUser(userId: number) {
    return this.http.delete(this.baseUrl + '/' + userId);
  }
  
  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

  getPermDict() {
    return this.http.get<object>(this.baseUrl + '/permdict');
  }
}