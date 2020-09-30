import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HTTP_INTERCEPTORS } from "@angular/common/http";
import {  Injectable } from "@angular/core";
import { Observable, throwError } from "rxjs";
import { catchError } from "rxjs/Operators";
@Injectable()
export class ErrorInterceptor implements HttpInterceptor{
 intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>>{
  return next.handle(req).pipe(
   catchError(error=>{
    if (error instanceof HttpErrorResponse) {
     const applicationError = error.headers.get('Application-error');
     if (applicationError) {
      console.error(applicationError);
      return throwError(applicationError);
     }
     //modle state error
     const serverError = error.error;
     let modleStateError = '';
     if (serverError && typeof serverError === 'object') {
      for (const key in serverError) {
       if (serverError[key]) {
        modleStateError+=serverError[key]+'\n'
       }
      }
     }
     return throwError(modleStateError||serverError||'Server Error')
   } 
   })
  )}
}
export const ErrorInterceptorProvider={
 provide:HTTP_INTERCEPTORS,
 useClass: ErrorInterceptor,
 multi:true
}
