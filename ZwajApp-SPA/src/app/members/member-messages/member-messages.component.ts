import { WriteKeyExpr } from '@angular/compiler';
import { AfterViewChecked, Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { Console } from 'console';
import { tap } from 'rxjs/Operators';
import { Message } from 'src/app/_models/message';
import { AuthService } from 'src/app/_services/Auth.service';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-member-messages',
  templateUrl: './member-messages.component.html',
  styleUrls: ['./member-messages.component.css']
})
export class MemberMessagesComponent implements OnInit,AfterViewChecked {
  @Input() recipientId: number;
  messages: Message[]
  newMessage: any = {}
  @ViewChild('panel') panel: ElementRef<any>;
  
 
  constructor(private authService: AuthService, private userService: UserService) { }
  ngAfterViewChecked(): void {
    this.panel.nativeElement.scrollTop = this.panel.nativeElement.scrollHeight;
  }

  ngOnInit() {
    this.loadMessages()
   
    this.authService.hubconnection.start();
    this.authService.hubconnection.on('refresh', () => {
  
        this.loadMessages();
    
    })
  }
  loadMessages() {
    const currentUserId = +this.authService.decodedToken.nameid
    this.userService.getConversation(this.authService.decodedToken.nameid, this.recipientId).pipe(tap(
      messages => {
        for (const message of messages) {
          if (message.isRead === false && message.recipientId === currentUserId) {
            this.userService.messageMarkAsRead(currentUserId, message.id)
          }
        }
      }
    )).subscribe(
      messages => {
        this.messages = messages.reverse();
      },
      error => { console.log("an error for loading messages") },
      () => {
        setTimeout(() => {
          this.userService.getUnreadcount(this.authService.decodedToken.nameid).subscribe(res => {
            this.authService.unreadCount.next(res.toString())
            setTimeout(() => {
              this.userService.getConversation(this.authService.decodedToken.nameid, this.recipientId).subscribe(messages => this.messages = messages);
          
            }, 3000);
          });
        },
        ), 1000
      }
    )
  };
  
  
  sendMessage() {
    this.newMessage.recipientId = this.recipientId
    this.userService.sendMessage(this.authService.decodedToken.nameid, this.newMessage).subscribe(
      (message: Message) => {
        this.messages.push(message);
        this.newMessage.content = '';
        this.authService.hubconnection.invoke('refresh')
      }
    )

   }
 

}

