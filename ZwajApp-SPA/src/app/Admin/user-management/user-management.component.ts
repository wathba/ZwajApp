import { Component, OnInit } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { User } from 'src/app/_models/user';
import { AdminService } from 'src/app/_services/admin.service';
import { RolesModalsComponent } from '../roles-modals/roles-modals.component';

@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.css']
})
export class UserManagementComponent implements OnInit {
  users: User[]
  bsModalRef: BsModalRef;
  constructor(private adminSerice:AdminService,private modalService: BsModalService) { }

  ngOnInit() {
   this.getUsersWithRoles()
  }
  getUsersWithRoles() {
    this.adminSerice.getUsersWithRoles().subscribe(
      (res:User[]) => {
        this.users=res
      },
      ()=> console.log('There is an errors for loading users')
   )
  }
  editRolesModal(user:User) {
    const initialState = {
      user,
      roles:this.getRolesArray(user) 
   
    };
    this.bsModalRef = this.modalService.show(RolesModalsComponent, {initialState});
    this.bsModalRef.content.updatedSelectedRoles.subscribe((values) => {
      const rolesToUpdate = {
        rolesName: [...values.filter(el => el.checked === true).map(el => el.value)]
      };
      if (rolesToUpdate) {
        this.adminSerice.updateUserRoles(user, rolesToUpdate).subscribe(
          () => {
            user.roles = [...rolesToUpdate.rolesName];
          },
          ()=> console.log('there is error')
        )
      }
    })


  }
  private getRolesArray(user) {
    const roles = [];
    const userRoles = user.roles as any[];
    const availableRoles: any[] = [
      {name: ' Administrator', value: 'Admin'},
      {name: 'Supervisor', value: 'Moderator'},
      {name: 'Member', value: 'Member'},
      {name: 'Subscriber', value: 'VIP'},
    ];

    availableRoles.forEach(aRole=>{
      let isMatch =false;
      userRoles.forEach(uRole=>{
        if(aRole.value===uRole){
          isMatch=true;
          aRole.checked = true;
          roles.push(aRole);
          return;
         }
      })
      if(!isMatch){
        aRole.checked=false;
        roles.push(aRole);
      }
    })
    return roles;
  }
}
