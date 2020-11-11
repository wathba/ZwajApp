import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService } from '../_services/Auth.service';


@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  /**
   *
   */
  constructor(private auth:AuthService,private router:Router) {
    
    
  }
  canActivate()
    : boolean {
    if (this.auth.loggedIn()) {
      this.auth.hubconnection.stop();
      return true;
    
    }
    else {
      console.log('You have to login')
      this.router.navigate(['/home'])
    
    }
      
    }
}
