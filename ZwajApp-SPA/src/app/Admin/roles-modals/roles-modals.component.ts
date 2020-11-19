import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap';
import { User } from 'src/app/_models/user';

@Component({
  selector: 'app-roles-modals',
  templateUrl: './roles-modals.component.html',
  styleUrls: ['./roles-modals.component.css']
})
export class RolesModalsComponent implements OnInit {

  user: User
  roles: any[]
  @Output() updatedSelectedRoles =new EventEmitter()
 
  constructor(public bsModalRef: BsModalRef) {}
  ngOnInit() {
  }
  updateRoles() {
    this.updatedSelectedRoles.emit(this.roles)
    this.bsModalRef.hide()
  }

}
