import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { map } from "rxjs/Operators";

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  jwtHelper = new JwtHelperService();
  decodedToken: any;
  baseUrl = 'http://localhost:5000/api/auth/';

  constructor(private http: HttpClient) { }
  login(model:any) {
    return this.http.post(this.baseUrl +'login', model).pipe(
      map((response:any)=>{
        const user = response;
        if (user) {
          localStorage.setItem('token', user.token);
          this.decodedToken = this.jwtHelper.decodeToken(user.token);
          console.log(this.decodedToken);
        }
      }))
  }
  register(modle: any) {
    return this.http.post(this.baseUrl + "register", modle);
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
