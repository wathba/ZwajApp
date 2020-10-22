import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Data } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from 'ngx-gallery';
import { User } from 'src/app/_models/user';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {
  user: User
    galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];
  showIntro:Boolean=true
  showLook:Boolean=true
  constructor(private userserivce:UserService, private route:ActivatedRoute ) { }

  ngOnInit() {
    // this.loadUser();
    this.route.data.subscribe(data => {
      this.user = data['user'];
      this.showIntro = true;
      this.showLook = true;
    });

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

}
