import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { map } from "rxjs/Operators";
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  jwtHelper = new JwtHelperService();
  decodedToken: any;
  baseUrl = 'http://localhost:5000/api/auth/';
  currentUser: User;
  constructor(private http: HttpClient) { }
  login(modle:any) {
    return this.http.post(this.baseUrl +'login', modle).pipe(
      map((response:any)=>{
        const user = response;
        if (user) {
          localStorage.setItem('token', user.token);
          localStorage.setItem('user',JSON.stringify(user.user))
          this.decodedToken = this.jwtHelper.decodeToken(user.token);
          this.currentUser = user.user;
          console.log(this.decodedToken);
        }
      }))
  }
  register(user:User) {
    return this.http.post(this.baseUrl + 'register', user);
  }
  loggedIn() {
    try{
    var token = localStorage.getItem('token');
      return !this.jwtHelper.isTokenExpired(token);
    }
    catch {
      return false
    }
  }

}
