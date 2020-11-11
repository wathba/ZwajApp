import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { User } from '../_models/user';
import { AuthService } from '../_services/Auth.service';
import { UserService } from '../_services/user.service';



@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
 model:any = {};
  count:string
  constructor(private authservice:AuthService,private userService:UserService, private route:Router) { }

  ngOnInit() {
    this.userService.getUnreadcount(this.authservice.decodedToken.nameid).subscribe(
      res => {
        this.authservice.unreadCount.next(res.toString())
        this.authservice.lastUnreadCount.subscribe(
          res=>this.count=res)}
    )
  
  }
  login(){
    this.authservice.login(this.model).subscribe(
      next => {
        alert('you are secceed to login'); this.userService.getUnreadcount(this.authservice.decodedToken.nameid).subscribe(
          res => {
            this.authservice.unreadCount.next(res.toString())
            this.authservice.lastUnreadCount.subscribe(
              res => this.count = res)
          }); },
      error => { console.log('your access denied') },
      ()=>{this.route.navigate(['/members'])}
    )
  }
  loggedin() {
    
    return this.authservice.loggedIn()
     
  }
  logout() {
    localStorage.removeItem('token');
    this.authservice.decodedToken = null;
    localStorage.removeItem('user');
    this.authservice.currentUser = null;
    console.log('You are signed out')
    this.route.navigate(['/home'])
  }
}
