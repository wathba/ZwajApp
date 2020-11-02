import { Component, Input, OnInit } from '@angular/core';
import { Data } from '@angular/router';
import { User } from 'src/app/_models/user';
import { AuthService } from 'src/app/_services/Auth.service';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-member-card',
  templateUrl: '././member-card.component.html',
  styleUrls: ['././member-card.component.css']
})
export class MemberCardComponent implements OnInit {
  @Input() user:User
  constructor(private userService:UserService,private authservice:AuthService) { }
  ngOnInit() {
  }
  sendLike(id:number) {
    this.userService.sendLike(this.authservice.decodedToken.nameid,id).subscribe(
      () => { console.log('You liked This Member');},
      error=>{console.log('there is error for this like')}
   )
 }
}
