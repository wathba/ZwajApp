import { Component, OnInit, ViewChild } from '@angular/core';
import { AllMemberReportComponent } from 'src/app/report/all-member-report/all-member-report.component';

@Component({
  selector: 'app-admin-panel',
  templateUrl: './admin-panel.component.html',
  styleUrls: ['./admin-panel.component.css']
})
export class AdminPanelComponent implements OnInit {
@ViewChild('report') report:AllMemberReportComponent
  constructor() { }

  ngOnInit() {
  }
  printAll() {
    this.report.printAll();
}
}
