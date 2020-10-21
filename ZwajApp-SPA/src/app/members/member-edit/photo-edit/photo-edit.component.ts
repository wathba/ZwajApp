import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { FileUploader } from 'ng2-file-upload';
import { error } from 'protractor';
import { Photo } from 'src/app/_models/photo';
import { User } from 'src/app/_models/user';
import { AuthService } from 'src/app/_services/Auth.service';
import { UserService } from 'src/app/_services/user.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-photo-edit',
  templateUrl: './photo-edit.component.html',
  styleUrls: ['./photo-edit.component.css']
})
export class PhotoEditComponent implements OnInit {
  @Input() photos: Photo[];
   
  uploader: FileUploader;
  hasBaseDropZoneOver:false;
  hasAnotherDropZoneOver: false;
  baseUrl = environment.apiUrl;
  currentMain: Photo;
  user: User;
 
  constructor (private authservive:AuthService,private route:ActivatedRoute, private userService:UserService){
  
  }
 


  ngOnInit() {
    this.IntializeFileUploader();
      this.route.data.subscribe(data => {
      this.user=data['user']
    })
  }
  public fileOverBase(e:any):void {
    this.hasBaseDropZoneOver = e;
  }
  IntializeFileUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'user/' + this.authservive.decodedToken.nameid + '/photos',
      authToken:'Bearer '+localStorage.getItem('token'),
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024
    });
    this.uploader.onAfterAddingFile = (file) => { file.withCredentials = false; };
    this.uploader.onSuccessItem = (item, Response, status, Headers) => {
      if(Response){const res: Photo = JSON.parse(Response);
        const photo = {
          id: res.id,
          url: res.url,
          isMain: res.isMain,
          dateAdded: res.dateAdded

        };
        this.photos.push(photo);
      }
      
    }
  }
  setMainPhoto(photo:Photo) {
    this.userService.setPhotoMain(this.authservive.decodedToken.nameid,photo.id).subscribe(
      ()=> {this.currentMain=this.photos.filter(p=>p.isMain===true)[0];
        this.currentMain.isMain=false;
        photo.isMain = true;
        this.user.photoUrl = photo.url;
        
      },
     ()=> console.log('photo is not main')
    )
  }
  delete(id: number) {
    if (window.confirm('Are you sure want to delete this photo')) {
      this.userService.deletePhoto(this.authservive.decodedToken.nameid, id).subscribe(
        () => {
          this.photos.splice(this.photos.findIndex(p => p.id === id), 1)
          alert('photo is deleted');
        }
      )
    }
  }
}
