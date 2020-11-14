import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
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
    if (this.loggedin) {
      this.userService.getUnreadcount(this.authservice.decodedToken.nameid).subscribe(
        res => {
          this.authservice.unreadCount.next(res.toString())
          this.authservice.lastUnreadCount.subscribe(
            res => this.count = res)
        }
      );
       this.getPaymentForUser();
    };
   
 
  
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
    this.getPaymentForUser();
  }
  loggedin() {
    
    return this.authservice.loggedIn()
     
  }
  logout() {
    localStorage.removeItem('token');
    this.authservice.decodedToken = null;
    this.authservice.paid = false;
    localStorage.removeItem('user');
    this.authservice.currentUser = null;
    console.log('You are signed out')
    this.route.navigate(['/home'])
  }
  getPaymentForUser() {
    this.userService.getPaymentForUser(this.authservice.currentUser.id).subscribe(
      res => {
        if (res == null) 
          this.authservice.paid=true
        else 
          this.authservice.paid=false
        
      }
    )
  }
}
