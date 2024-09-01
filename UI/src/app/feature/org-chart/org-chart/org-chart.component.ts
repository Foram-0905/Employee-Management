import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { OrgChartService } from '../../../shared/services/org-chart.service';

@Component({
  selector: 'app-org-chart',
  templateUrl: './org-chart.component.html',
  styleUrls: ['./org-chart.component.scss']
})
export class OrgChartComponent implements OnInit {
  employees: any;
  data: any;

  constructor(
    private translateService: TranslateService,
    private orgchartservice: OrgChartService
  ) {
    this.translateService.setDefaultLang('en');
    this.translateService.use(localStorage.getItem('lang') || 'en');
  }

  ngOnInit(): void {
    this.getEmployees();
  }

  getEmployees() {
    this.orgchartservice.getEmployee().subscribe({
      next: (data: any) => {
        this.employees = data.httpResponse;
        this.data = this.buildHierarchy(this.employees);
        console.log("Employee Data", this.data);
      }
    });
  }

  buildHierarchy(employeeGroups: any): any[] {
    return employeeGroups.map((group: any) => ({
      name: group.group,
      employees: group.names
    }));
  }
}
