import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve, Router  } from "@angular/router";
import { Observable, of } from "rxjs";
import { catchError } from "rxjs/Operators";
import { User } from "../_models/user";
import { AuthService } from "../_services/Auth.service";
import { UserService } from "../_services/user.service";

@Injectable()
export class MemberEditResolver implements Resolve<User>{
 
 constructor(private userService:UserService, private router: Router,private authservice:AuthService) { }
 resolve(route: ActivatedRouteSnapshot):Observable<User>{
  return this.userService.getUser(this.authservice.decodedToken.nameid).pipe(
   catchError(error=>{
    console.log('there is an error ');
    this.router.navigate(['/members']);
    return of(null);

   })
  )
  
 }
 
}