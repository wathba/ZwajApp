import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Pagination, PaginationResult } from '../_models/Pagination';
import { User } from '../_models/user';
import { AuthService } from '../_services/Auth.service';
import { UserService } from '../_services/user.service';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.css']
})
export class ListComponent implements OnInit {
  users: User[]
  pagination: Pagination
  likeParam: string
  search:boolean=false
  constructor(private authService:AuthService,private userService:UserService,private route:ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(
      data => {
        this.users = data['users'].result;
        this.pagination = data['users'].pagination;
      });
    this.likeParam = 'likers';
  }
  loadUsers() {
    if (!this.search) {
      this.pagination.currentPage=1
    }
    this.userService.getUsers(this.pagination.currentPage,this.pagination.itemPerPage,null,this.likeParam).subscribe((res:PaginationResult<User[]>)=>{
      this.users = res.result;
      this.pagination = res.pagination;
    },
      error=>console.log('there is an eror')
    )
}
  pageChanged(event: any): void {
     this.pagination.currentPage = event.page;
     this.loadUsers();
    

  }
}
