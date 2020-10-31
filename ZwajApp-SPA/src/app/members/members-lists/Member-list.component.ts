import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot, Router } from '@angular/router';
import { Pagination, PaginationResult } from 'src/app/_models/Pagination';
import { User } from '../../_models/user';
import { UserService } from '../../_services/user.service';

@Component({
  selector: 'app-Member-list',
  templateUrl: './Member-list.component.html',
  styleUrls: ['./Member-list.component.css']
})
export class MemberListComponent implements OnInit {
  users: User[];
  user: User = JSON.parse(localStorage.getItem('user'));
  genderList = [{ value: 'male', display: 'Mens' }, { value: 'female', display: 'Womens' }];
  userParams: any = {};
  pagination:Pagination
  constructor(private userService:UserService,private route:ActivatedRoute) { }

  ngOnInit() {
  //  this.usersLoad();
     this.route.data.subscribe(data=>{
       this.users = data['users'].result
       this.pagination = data['users'].pagination
       
     });
    this.userParams.gender = this.user.gender === 'male' ? 'female' : 'male';
    this.userParams.minAge = 18;
    this.userParams.maxAge = 99;
    this.userParams.orderBy = 'lastActive';
  }
  resetFilter() {
      this.userParams.gender = this.user.gender === 'male' ? 'female' : 'male';
    this.userParams.minAge = 18;
    this.userParams.maxAge = 99;
   
     this.loadUsers();
  }
   pageChanged(event: any): void {
     this.pagination.currentPage = event.page;
     this.loadUsers();
    

  }
  loadUsers(){
    this.userService.getUsers(this.pagination.currentPage,this.pagination.itemPerPage,this.userParams).subscribe((res:PaginationResult<User[]>)=>{
      this.users = res.result;
      this.pagination = res.pagination;
    },
      error=>console.log('there is an eror')
    )
}
}
