import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { httpFactory } from '@angular/http/src/http_module';

@Component({
  selector: 'app-values',
  templateUrl: './values.component.html',
  styleUrls: ['./values.component.css']
})
export class ValuesComponent implements OnInit {
  values:any;

  constructor(private http:HttpClient) { }

  ngOnInit() {
    this.geValues();
  }
   geValues() {
     this.http.get('http://localhost:5000/api/values').subscribe(
       response => { this.values = response; },
       error => { console.log(error);}
    )
  }

}
