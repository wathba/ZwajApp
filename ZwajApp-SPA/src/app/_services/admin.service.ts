import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  baseUrl = environment.apiUrl + ('admin/')
  constructor(private http: HttpClient) { }
  getUsersWithRoles() {
    return this.http.get(this.baseUrl + 'userwithroles')
  }
  updateUserRoles(user:User,roles:{}) {
    return this.http.post(this.baseUrl+'editroles'+user.userName,roles)
  }
}
