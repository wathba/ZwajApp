import { Component, OnInit } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { User } from './_models/user';
import { AuthService } from './_services/Auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
}) 
 
export class AppComponent implements OnInit{
   jwtHelper = new JwtHelperService();
  constructor(private authservice:AuthService) { }

  ngOnInit() {
    var token = localStorage.getItem('token');
    if (token) {
      this.authservice.decodedToken = this.jwtHelper.decodeToken(token);
    }
    
    var user:User =JSON.parse(localStorage.getItem('user'))
    if (user) {
      this.authservice.currentUser=user
    }
    
  
  }
}
