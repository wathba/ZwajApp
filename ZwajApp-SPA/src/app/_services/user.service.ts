import { HttpClient, HttpHandler, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';
import { AuthService } from './Auth.service';


@Injectable({
  providedIn: 'root'
})
export class UserService {
  baseUrl = environment.apiUrl+'user/';

  constructor(private http: HttpClient,private authserive: AuthService) { }
     getUsers():Observable<User[]>{
       return this.http.get<User[]>(this.baseUrl);
  };
  getUser(id): Observable<User>{
   return this.http.get<User>(this.baseUrl+id)
  };
  updateForUser(id: number, user: User) {
    return this.http.put(this.baseUrl+id,user);
  }
  setPhotoMain(userId: number, id: number) {
    return this.http.post(this.baseUrl+ userId+'/photos/'+id+'/setmain',{});
  }
  deletePhoto(userId: User, id: number) {
    return this.http.delete(this.baseUrl + userId + '/photos/' + id);
  }
}
