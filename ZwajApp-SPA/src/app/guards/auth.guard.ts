import { Injectable } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot, CanActivate, Router } from '@angular/router';
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
  canActivate(next:ActivatedRouteSnapshot)
    : boolean {
    
    const roles = next.data['role'] as Array<string>
    if (roles) {
      const match = this.auth.roleMatch(roles)
      if (match) {
        return true
      }
      else {
        this.router.navigate(['/member'])
      }
    }
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
