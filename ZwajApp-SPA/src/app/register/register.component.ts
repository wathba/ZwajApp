import { Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import { AuthService } from '../_services/Auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  model: any = {};
  @Input() valuefromRegister: any;
  @Output() cancelRegister = new EventEmitter();
  constructor(private auth:AuthService) { }

  ngOnInit() {
  }
  register() {
    this.auth.register(this.model).subscribe(
      ()=>{ console.log('you have been register') },
      error=>{ console.log(console.error) }
    )
  }
  cancel() {
    console.log('Not Now');
    this.cancelRegister.emit(false);
  }

}
