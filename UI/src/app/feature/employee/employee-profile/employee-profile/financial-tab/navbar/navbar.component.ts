import { Component, ViewContainerRef, ComponentFactoryResolver, Type, OnInit } from '@angular/core';
import { SalaryComponent } from './salary/salary.component';
import { BonusComponent } from './bonus/bonus.component';
import { ConsultantRateComponent } from './consultant-rate/consultant-rate.component';
import { EmployeeService } from '../../../../../../shared/services/employee.service';
import { first } from 'rxjs';
import { EmployementType } from '../../../../../../shared/constant/enum.const';
import { GetEmployeeById } from '../../../../../../shared/constant/api.const';
import { EmployeeFlagService } from '../../../../../../shared/services/EmployeFlag.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-fnavbar',
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss'
})
export class FNavbarComponent implements OnInit{
  activeTab: string = '';
  selectedEmployeeId!: string;
  employmentType: string = '';
  employmentTypeId: string = '';
  constructor(
    private vcr: ViewContainerRef,
    private resolver: ComponentFactoryResolver,
    private employeeService: EmployeeService,
    private EmployeFlage: EmployeeFlagService,
   private spinner: NgxSpinnerService,
   private toastr: ToastrService

  ) {
    
  
  }

  ngOnInit(): void {
    this.EmployeFlage.selectedEmployee$.subscribe((employee) => {
      if (employee) {
        this.selectedEmployeeId = employee;
        this.getConsultantByEmployeeId();
      
      }
    });
  }

  toggleActive(tab: string): void {
    this.activeTab = tab;
    switch (tab) {
      case 'salary':
        this.loadComponent(SalaryComponent);
        break;
      case 'bonus':
        this.loadComponent(BonusComponent);
        break;
      case 'consultant-rate':
        this.loadComponent(ConsultantRateComponent);
        break;
      default:
        break;
    }
  }

  async loadComponent(componentType: Type<any>): Promise<void> {
    this.vcr.clear();
    if (!componentType) {
      return;
    }
    const componentFactory = this.resolver.resolveComponentFactory(componentType);
    this.vcr.createComponent(componentFactory);
  }

  getConsultantByEmployeeId(): void {
    this.spinner.show();
    this.employeeService
      .getEmployeeById(this.selectedEmployeeId)
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.employmentTypeId = resp.httpResponse.employementTypeId || '';
          console.log('Employment Type ID:', this.employmentTypeId);
          this.setDefaultActiveTab();
          this.spinner.hide();
        },
        error: (err: any) => {
          this.spinner.hide();
         this.toastr.error(err.error.message);
        }
      });
  }

  setDefaultActiveTab(): void {
    if (this.employmentTypeId === EmployementType.Consultant) {
   //   this.spinner.hide();
      this.toggleActive('consultant-rate');
    } else if (this.employmentTypeId === EmployementType.Corporate) {
     // this.spinner.hide();
      this.toggleActive('salary');
    }
  }

  shouldDisplayTab(tab: string): boolean {
    if (this.employmentTypeId === EmployementType.Consultant) {
   //   this.spinner.hide();
      return tab === 'consultant-rate';
    } else if (this.employmentTypeId === EmployementType.Corporate) {
    //  this.spinner.hide();
      return tab === 'salary' || tab === 'bonus';
    }
    return false;
  }
  
}
