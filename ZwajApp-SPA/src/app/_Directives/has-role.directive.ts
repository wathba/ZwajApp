import { Directive, Input, OnInit, TemplateRef, ViewContainerRef } from '@angular/core';
import { AuthService } from '../_services/Auth.service';


@Directive({
  selector: '[hasRole]'
})
export class HasRoleDirective implements OnInit {
  @Input() hasRole: string[]
   isVisible =false
  constructor(private viewContainerRef:ViewContainerRef,private templateRef:TemplateRef<any>,private authService:AuthService) { }
  ngOnInit() {
    const userRoles = this.authService.decodedToken.role as string[]
    if (!userRoles) {
      this.viewContainerRef.clear();
      if (this.authService.roleMatch(this.hasRole)) {
        if (!this.isVisible) {
          this.isVisible = true
          this.viewContainerRef.createEmbeddedView(this.templateRef)
        }
        else {
          this.isVisible=false
        }
      }
    }
  }

}
