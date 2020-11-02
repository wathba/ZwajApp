import { HttpClient, HttpHandler, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/Operators';
import { environment } from 'src/environments/environment';
import { PaginationResult } from '../_models/Pagination';
import { User } from '../_models/user';
import { AuthService } from './Auth.service';


@Injectable({
  providedIn: 'root'
})
export class UserService {
  baseUrl = environment.apiUrl+'user/';

  constructor(private http: HttpClient,private authserive: AuthService) { }
  getUsers(page?,itemPerPage?,userParams?,likeParam?): Observable<PaginationResult<User[]>>{
    const paginationResult: PaginationResult<User[]> = new PaginationResult<User[]>();
    let params = new HttpParams();
    if (page != null && itemPerPage != null) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemPerPage);
      
    }
    if (userParams != null) {
      params =params.append('minAge',userParams.minAge)
      params =params.append('maxAge',userParams.maxAge)
      params = params.append('gender', userParams.gender)
      params = params.append('orderBy', userParams.orderBy)
    }
    if (likeParam ==='likers') {
      params=params.append('likers','true')
    }
    if (likeParam ==='likees') {
      params=params.append('likees','true')
    }

    return this.http.get<User[]>(this.baseUrl, { observe: 'response', params }).pipe(
      map(response =>{
        paginationResult.result = response.body;
        if (response.headers.get('Pagination')!= null) {
          paginationResult.pagination = JSON.parse(response.headers.get('Pagination'));
        }
        return paginationResult;
         })
       )
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
  sendLike(id: number, recipient: number) {
    return this.http.post(this.baseUrl+id+'/like/'+recipient,{});
  }
}