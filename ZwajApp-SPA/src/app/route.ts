import {Routes} from '@angular/router'
import { AuthGuard } from './guards/auth.guard';
import {HomeComponent} from './home/home.component';
import { ListComponent } from './list/list.component';
import { MemberDetailComponent } from './members/members-lists/member-detail/member-detail.component';
import { MemberListComponent } from './members/members-lists/Member-list.component';
import { MessagesComponent } from './messages/messages.component';
import { MemberDetailsResolver } from './_resolver/member-details-resolver';
import { MemberListResolver } from './_resolver/member-list-resolver';
export const appRoutes:Routes=[
 {path:'',component:HomeComponent},
 {path:'home',component:HomeComponent},
 {
  path: 'members/:id', component:MemberDetailComponent,resolve:{
  user:MemberDetailsResolver
 }},
 {
  path: 'members', component: MemberListComponent, resolve: {
   users :MemberListResolver  
  },
  canActivate: [AuthGuard]
 },
 { path: 'lists', component:ListComponent},
 {path:'messages',component:MessagesComponent},
 {path:'**',redirectTo:'home',pathMatch:'full'}
]