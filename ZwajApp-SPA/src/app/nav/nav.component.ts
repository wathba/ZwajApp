import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../_services/Auth.service';



@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};
  
  constructor(private authservice:AuthService, private route:Router) { }

  ngOnInit() {
  
  }
  login(){
    this.authservice.login(this.model).subscribe(
      next=>{ alert('you are secceed to login') },
      error => { console.log('your access denied') },
      ()=>{this.route.navigate(['/members'])}
    )
  }
  loggedin() {
    
    return this.authservice.loggedIn()
     
  }
  logout() {
    localStorage.removeItem('token');
    
    console.log('You are signed out')
    this.route.navigate(['/home'])
  }
}
