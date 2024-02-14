import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../environment';
import { User, UserAdd } from '../_models/user';
import { PermissionProfile } from '../_models/PermissionProfile';

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

  getPermissionProfiles() {
    return this.http.get<PermissionProfile[]>(this.baseUrl + '/permissionProfiles');
  }

  addPermissionProfile(profileName: string, permission: string) {
    return this.http.put<PermissionProfile>(this.baseUrl + '/permissionProfiles/add', {profileName, permission});
  }

  deletePermissionProfile(id: number) {
    return this.http.delete(this.baseUrl + '/permissionProfiles/' + id);
  }
}
