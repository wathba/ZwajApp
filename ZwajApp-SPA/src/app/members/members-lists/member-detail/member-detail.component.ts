import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Data } from '@angular/router';
import { TabsetComponent } from 'ngx-bootstrap/tabs';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from 'ngx-gallery';
import { User } from 'src/app/_models/user';
import { AuthService } from 'src/app/_services/Auth.service';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {
  @ViewChild('memberTabs') memberTabs:TabsetComponent
  user: User
    galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];
  showIntro:Boolean=true
  showLook:Boolean=true
  constructor(private userserivce:UserService,private authService:AuthService, private route:ActivatedRoute ) { }

  ngOnInit() {
    // this.loadUser();
    this.authService.hubconnection.stop();
    this.route.data.subscribe(data => {
      this.user = data['user'];
      this.showIntro = true;
      this.showLook = true;
    });
      this.route.queryParams.subscribe(
      params => {
        const selectedTab = params['tab']
        this.memberTabs.tabs[selectedTab>0?selectedTab:0].active=true
      }
    )

    this.galleryOptions = [{
      width: '500px', height: '500px', imagePercent: 100, thumbnailsColumns: 4,
      imageAnimation: NgxGalleryAnimation.Slide, preview: false
    }];
    this.galleryImages = this.getImages();
   }
  // loadUser() {
  //   this.userserivce.getUser(+this.route.snapshot.params['id']).subscribe(
  //     (user:User)=>{ this.user = user },
  //     error => console.log('some errors')
      
//   )
// }
  getImages() {
    const imageUrls = [];
    for (let i = 0; i < this.user.photos.length; ++i){
      imageUrls.push({

          small: this.user.photos[i].url,
          medium: this.user.photos[i].url,
          big: this.user.photos[i].url,
      })
    }
    return imageUrls;
  }
  selectedTab(tabId: number) {
    this.memberTabs.tabs[tabId].active = true;
  }
  deSelect() {
    this.authService.hubconnection.stop();
  }

}
