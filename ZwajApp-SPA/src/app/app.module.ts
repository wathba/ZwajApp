import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { BsDropdownModule, ButtonsModule, ModalModule, PaginationModule, TabsetComponent, TabsModule } from 'ngx-bootstrap';
import { JwtModule } from '@auth0/angular-jwt';
import { FileUploadModule } from 'ng2-file-upload';

import { AppComponent } from './app.component';

import { NavComponent } from './nav/nav.component';
import { AuthService } from './_services/Auth.service';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { ErrorInterceptorProvider } from './_services/error.interceptor';
import { MemberListComponent } from './members/members-lists/Member-list.component';
import { ListComponent } from './list/list.component';
import { MessagesComponent } from './messages/messages.component';
import { RouterModule } from '@angular/router';
import { appRoutes } from './route';
import { AuthGuard } from './guards/auth.guard';
import { UserService } from './_services/user.service';
import { MemberCardComponent } from './members/members-lists/member-card/member-card.component';
import { MemberDetailComponent } from './members/members-lists/member-detail/member-detail.component';
import { MemberDetailsResolver } from './_resolver/member-details-resolver';
import { MemberListResolver } from './_resolver/member-list-resolver';
import { NgxGalleryModule } from 'ngx-gallery';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { MemberEditResolver } from './_resolver/member-edit-resolver';
import { PhotoEditComponent } from './members/member-edit/photo-edit/photo-edit.component';
import { ListResolver } from './_resolver/lists.resolver';
import { MessageResolver } from './_resolver/message-resolver';
import { MemberMessagesComponent } from './members/member-messages/member-messages.component';
import { PaymentsComponent } from './payments/payments.component';
import { MessagesGuard } from './guards/messages.guard';
import { ChargeGuard } from './guards/charge.guard';
import { AdminPanelComponent } from './Admin/admin-panel/admin-panel.component';
import { HasRoleDirective } from './_Directives/has-role.directive';
import { UserManagementComponent } from './Admin/user-management/user-management.component';
import { PhotoManagementComponent } from './Admin/photo-management/photo-management.component';
import { AdminService } from './_services/admin.service';
import { RolesModalsComponent } from './Admin/roles-modals/roles-modals.component';






export function tokenGetter() {
  return localStorage.getItem('token');
}

@NgModule({
  declarations: [									
    AppComponent,
    
      NavComponent,
      HomeComponent,
      RegisterComponent,
      MemberListComponent,
      ListComponent,
    MessagesComponent,
    MemberCardComponent,
    MemberDetailComponent,
    MemberEditComponent,
    PhotoEditComponent,
    MemberMessagesComponent,
    PaymentsComponent,
    AdminPanelComponent,
    HasRoleDirective,
    UserManagementComponent,
    PhotoManagementComponent,
    RolesModalsComponent,
     
    
   ],
  imports: [
    BrowserModule,
    FormsModule,
    NgxGalleryModule,
    FileUploadModule,
    ReactiveFormsModule,
     PaginationModule.forRoot(),
    BsDropdownModule.forRoot(),
    ButtonsModule,
    RouterModule.forRoot(appRoutes),
    TabsModule.forRoot(),
    HttpClientModule,
    ModalModule.forRoot(),
     JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        whitelistedDomains:['localhost:5000'],
       blacklistedRoutes: ["http://localhost:5000/api/auth"]
      },
    }),
     
  ],
  entryComponents:[RolesModalsComponent],
  providers: [AuthService,ErrorInterceptorProvider,AuthGuard ,MessagesGuard,ChargeGuard,UserService,AdminService ,MemberDetailsResolver,MemberListResolver,MemberEditResolver,ListResolver,MessageResolver],
  bootstrap: [AppComponent]
})
export class AppModule { }
