import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { JwtHelperService } from '@auth0/angular-jwt';
import { BehaviorSubject } from 'rxjs';
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
   paid: boolean = false;
  siteLang: string = 'en'
  dir:string='ltr'
  unreadCount = new BehaviorSubject<string>('');
  lastUnreadCount = this.unreadCount.asObservable();
  language = new BehaviorSubject<string>('en')
  lang = this.language.asObservable();
 
   hubconnection:HubConnection = new HubConnectionBuilder().withUrl('http:localhost:5000/chat').build();
  constructor(private http: HttpClient) { 
    this.lang.subscribe(lang => {
      if (lang == 'en') {
        this.dir = 'ltr'
        this.siteLang='en'
      } else {
          this.dir = 'rtl'
        this.siteLang='ar'
      }
    })
  }
  login(model:any) {
    return this.http.post(this.baseUrl +'login', model).pipe(
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
  roleMatch(AllowedRoles: Array<string>): boolean{
    let isMatch = false
    const userRoles = this.decodedToken.role as Array<string>
    AllowedRoles.forEach(element => {
      if (userRoles.includes(element)) {
        isMatch = true
        return;
      }
    });
    return isMatch;
  }

}
