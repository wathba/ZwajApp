
import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve, Router  } from "@angular/router";
import { Observable, of } from "rxjs";
import { catchError } from "rxjs/Operators";
import { Message } from "../_models/message";
import { User } from "../_models/user";
import { AuthService } from "../_services/Auth.service";
import { UserService } from "../_services/user.service";

@Injectable()
export class MessageResolver implements Resolve<Message[]>{
 pageNumber = 1
 pageSize = 6
 messageType='Unread'
 
 constructor(private userService:UserService, private router: Router,private authService:AuthService) { }
 resolve(route: ActivatedRouteSnapshot):Observable<Message[]>{
  return this.userService.getMessages(this.authService.decodedToken.nameid,this.pageNumber,this.pageSize,this.messageType).pipe(
   catchError(error=>{
    console.log('there is an error ');
    this.router.navigate(['']);
    return of(null);

   })
  )
  
 }
 
}