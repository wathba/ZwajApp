import { Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { User } from '../_models/user';

import { AuthService } from '../_services/Auth.service';


@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  user:User;
  @Input() valuefromRegister: any;
  @Output() cancelRegister = new EventEmitter();
  registerForm:FormGroup
  constructor(private auth:AuthService,private fb:FormBuilder,private router:Router) { }

  ngOnInit() {
    // this.registerForm = new FormGroup(
    // {  username: new FormControl('',Validators.required),
    //   password: new FormControl('',[Validators.required,Validators.minLength(4),Validators.maxLength(8)]),
    //   confirmPassword: new FormControl('',Validators.required)},this.passwordMatchValidator)
    this.createRegisterform();
  }
  createRegisterform() {
    this.registerForm = this.fb.group({
      gender: ['Man'],
      userName: ['', [Validators.required, Validators.minLength(5)]],
      knownAs: ['', Validators.required],
      dateOfBirth: [null, Validators.required],
      city: ['', Validators.required],
      country:['',Validators.required],
      password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
      confirmPassword:['',[Validators.required, Validators.minLength(4), Validators.maxLength(8)]]
    },{validator : this.passwordMatchValidator})
     
    
  }
  passwordMatchValidator(form: FormGroup) {
    return  form.get('password').value === form.get('confirmPassword').value ? null : {'mismatch':true}
  
    }
    
    
  
  register() {
    if (this.registerForm.valid) {
      this.user = Object.assign({}, this.registerForm.value)
      
      this.auth.register(this.user).subscribe(
        () => { console.log('you have been register') },
        error => { console.log('there is mistake') },
        ()=>{ this.auth.login(this.user).subscribe(
          () => { this.router.navigate(['/members']);}
          )}
      )}
console.log(this.registerForm.value) }
  cancel() {
    console.log('Not Now');
    this.cancelRegister.emit(false);
  }

}
