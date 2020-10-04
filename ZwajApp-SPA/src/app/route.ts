import {Routes} from '@angular/router'
import { AuthGuard } from './guards/auth.guard';
import {HomeComponent} from './home/home.component';
import { ListComponent } from './list/list.component';
import { MemberListComponent } from './members-lists/Member-list.component';
import { MessagesComponent } from './messages/messages.component';
export const appRoutes:Routes=[
 {path:'',component:HomeComponent},
 {path:'home',component:HomeComponent},
 {path:'members',component:MemberListComponent,canActivate:[AuthGuard]},
 { path: 'lists', component:ListComponent},
 {path:'messages',component:MessagesComponent},
 {path:'**',redirectTo:'home',pathMatch:'full'}
]