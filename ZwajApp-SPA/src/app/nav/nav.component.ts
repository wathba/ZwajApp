import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/Auth.service';



@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};
  
  constructor(private authservice:AuthService) { }

  ngOnInit() {
  
  }
  login(){
    this.authservice.login(this.model).subscribe(
      next=>{ console.log('you are secceed to login') },
      error=>{console.log('your access denied')}
    )
  }
  loggedin() {
    
    return  this.authservice.loggedIn()
    
  }
  logout() {
    localStorage.removeItem('token');
    
    console.log('You are signed out')
  }
}
