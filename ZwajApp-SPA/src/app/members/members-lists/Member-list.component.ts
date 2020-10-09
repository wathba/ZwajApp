import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot, Router } from '@angular/router';
import { User } from '../../_models/user';
import { UserService } from '../../_services/user.service';

@Component({
  selector: 'app-Member-list',
  templateUrl: './Member-list.component.html',
  styleUrls: ['./Member-list.component.css']
})
export class MemberListComponent implements OnInit {
  users: User[];
  constructor(private userService:UserService,private route:ActivatedRoute) { }

  ngOnInit() {
  //  this.usersLoad();
     this.route.data.subscribe(data=>{
      this.users=data['users'];
    });
  }
//   usersLoad(){
//     this.userService.getUsers().subscribe((users:User[])=>{
//     this.users=users
//     },
//       error=>console.log(error)
//     )
// }
}
