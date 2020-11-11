
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HubConnection,HubConnectionBuilder } from '@aspnet/signalr';
import { Message } from '../_models/message';
import { Pagination, PaginationResult } from '../_models/Pagination';
import { AuthService } from '../_services/Auth.service';
import { UserService } from '../_services/user.service';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {
  messages :Message[];
  pagination: Pagination
  messageType = 'Unread'
  hubConnection: HubConnection
  count :string

  constructor(private authserive:AuthService,private userService:UserService,private route:ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(
      data => {
        this.messages = data['messages'].result
        this.pagination=data['messages'].pagination
      }
    )
    this.hubConnection = new HubConnectionBuilder().withUrl('http:localhost:5000/chat').build();
    this.hubConnection.start();
    this.hubConnection.on('count', () => {
      setTimeout(() => {
        this.userService.getUnreadcount(this.authserive.decodedToken.nameid).subscribe(res => {
          this.authserive.unreadCount.next(res.toString());
          this.authserive.lastUnreadCount.subscribe(res=>this.count=res)
        })
      }, 0);
    })
  this.loadMessages() 
  }
  loadMessages() {
    this.userService.getMessages(this.authserive.decodedToken.nameid, this.pagination.currentPage, this.pagination.itemPerPage, this.messageType).subscribe(
      (res:PaginationResult<Message[]>)=> {
        this.messages = res.result;
        this.pagination =res.pagination
      },
      ()=>console.log('an Error for loading messages')
    )
  }
  pageChanged(event: any): void {
    this.pagination.currentPage=event.page
  this.loadMessages()
  }
   deleteMessage(id) {
    confirm('Do You want to Delete Message')
    this.userService.deleteMessage(id, this.authserive.decodedToken.nameid).subscribe(
      () => {
        this.messages.splice(this.messages.findIndex(m => m.id == id), 1);
        alert('message Deleted')
      },
      ()=> alert('There is problem  happend at deleting')
    );

  } 
}
