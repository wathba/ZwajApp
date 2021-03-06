import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve, Router  } from "@angular/router";
import { Observable, of } from "rxjs";
import { catchError } from "rxjs/Operators";
import { User } from "../_models/user";
import { UserService } from "../_services/user.service";

@Injectable()
export class MemberListResolver implements Resolve<User[]>{
 pageNumber = 1
 pageSize=6
 
 constructor(private userService:UserService, private router: Router) { }
 resolve(route: ActivatedRouteSnapshot):Observable<User[]>{
  return this.userService.getUsers(this.pageNumber,this.pageSize).pipe(
   catchError(error=>{
    console.log('there is an error ');
    this.router.navigate(['']);
    return of(null);

   })
  )
  
 }
 
}