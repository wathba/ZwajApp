import { Component, HostListener, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute} from '@angular/router';
import { User } from 'src/app/_models/user';
import { AuthService } from 'src/app/_services/Auth.service';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
  user: User
  editForm: NgForm
  @HostListener('window:beforeUnload', ['$event'])
  unloadNotification($event: any) {
    if (this.editForm.dirty) {
      $event.retrunValue=true
    }
    }
  constructor(private route:ActivatedRoute,private userservice:UserService, private authservice:AuthService) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.user=data['user']
    })
    this.authservice.hubconnection.stop();
  }
  updateUser() {
    
    this.userservice.updateForUser(this.authservice.decodedToken.nameid,this.user).subscribe(()=>{
      alert('User Successfuly Updated');
      this.editForm.reset(this.user);
    }, error => alert('there is problem user not updated'));
    
}
}
