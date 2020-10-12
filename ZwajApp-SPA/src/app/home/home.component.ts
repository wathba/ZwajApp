import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../_services/Auth.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  registerMode: boolean = false;
 
  constructor(private http:HttpClient, private authservice:AuthService,private router:Router) { }

  ngOnInit() {
    if (this.authservice.loggedIn) {
      this.router.navigate(['/members'])
    }
    
  }
  registerToggle() {
    this.registerMode = true;
  }
   
  cancelRegister(mode: boolean) {
    this.registerMode = mode;
  }


}
