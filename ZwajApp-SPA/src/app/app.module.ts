import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from "@angular/forms";
import { BsDropdownModule, TabsModule } from 'ngx-bootstrap';
import { JwtModule } from '@auth0/angular-jwt';

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
    
   ],
  imports: [
    BrowserModule,
    FormsModule,
     NgxGalleryModule,
    BsDropdownModule.forRoot(),
    RouterModule.forRoot(appRoutes),
    TabsModule.forRoot(),
    HttpClientModule,
     JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        whitelistedDomains:['localhost:5000'],
       blacklistedRoutes: ["http://localhost:5000/api/auth"]
      },
    }),
     
  ],
  providers: [AuthService,ErrorInterceptorProvider,AuthGuard ,UserService, MemberDetailsResolver,MemberListResolver,MemberEditResolver],
  bootstrap: [AppComponent]
})
export class AppModule { }
