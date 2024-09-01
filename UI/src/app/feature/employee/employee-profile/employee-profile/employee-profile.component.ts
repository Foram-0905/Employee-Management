import { Component,ViewContainerRef,ComponentFactoryResolver, OnInit, EventEmitter, Output } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { EmployeeService } from '../../../../shared/services/employee.service';
import { Form, FormBuilder, FormGroup } from '@angular/forms';
import {EmployeeFlagService} from '../../../../shared/services/EmployeFlag.service'

@Component({
  selector: 'app-employee-profile',
  templateUrl: './employee-profile.component.html',
  styleUrl: './employee-profile.component.scss'
})
export class EmployeeProfileComponent implements OnInit{
 
  getActiveTabIndex: number = 0;
  flag: boolean = false;
  Editflag: boolean = true;
  currentId: any;
  keyword = 'fullName';
  frm!: FormGroup;
  GetEmployeeData: any[] = [];
  SelectedEmployee: any;

  constructor(
    private vcr: ViewContainerRef,
    private route: ActivatedRoute,
    private employee: EmployeeService,
    private EmployeFlag: EmployeeFlagService,
    private FB: FormBuilder,
    private resolver: ComponentFactoryResolver,
    private translateService: TranslateService
  ) {}

  ngOnInit(): void {
    const employeeParams = this.EmployeFlag.getEmployeeParams();
    this.currentId = employeeParams.id;
    this.Editflag = employeeParams.Editflag !== undefined ? employeeParams.Editflag : true;
    console.warn('Editflag', this.Editflag);

    

    // Retrieve selected employee first
      this.retrieveSelectedEmployee();
      
      this.EmployeFlag.setSelectedEmployee(this.currentId);
    

    // Initialize form with the retrieved currentId value
    this.frm = this.FB.group({
      'SelectedEmploye': [this.currentId]
    });

    // Subscribe to changes on the form control and update localStorage
    this.frm.get('SelectedEmploye')?.valueChanges.subscribe(value => {
      this.currentId = value;
      localStorage.setItem('SelectedEmployeeForEdit', JSON.stringify(value));
      this.EmployeFlag.setSelectedEmployee(value);
    });

   
    this.route.queryParams.subscribe(params => {
      this.flag = params['flag'] === 'true';
    });

    // Check the value of flag and set the form control value accordingly
    if (this.flag) {
      this.frm.patchValue({
        'SelectedEmploye': null
      });
    }

    //this.checkAndRetrieveEmployee();

    this.GetEmployee();
   // this.SendEmployeIdToService();
    this.personal();
    this.translateService.setDefaultLang('en');
    this.translateService.use(localStorage.getItem('lang') || 'en');
}


retrieveSelectedEmployee(): void {
  let selectedEmployeeJson: string | null = localStorage.getItem('SelectedEmployeeForEdit');
  let selectedEmployee = selectedEmployeeJson ? JSON.parse(selectedEmployeeJson) : null;

  if (this.Editflag && selectedEmployee !== null) {
    if (typeof selectedEmployee === 'string') {
      selectedEmployee = selectedEmployee.replace(/^"|"$/g, '');
    }
    // Convert to lowercase if selectedEmployee is a string
    if (typeof selectedEmployee === 'string') {
      selectedEmployee = selectedEmployee.toLowerCase();
    }

    this.SelectedEmployee = selectedEmployee;
    this.currentId = selectedEmployee;
    console.warn('truevalue', this.SelectedEmployee);
  } else {
    // If Editflag is false or selectedEmployee is null, retain the currentId value
    // and update localStorage with the currentId
    selectedEmployee = this.currentId; // Use the currentId value
    localStorage.setItem('SelectedEmployeeForEdit', JSON.stringify(selectedEmployee));
    this.SelectedEmployee = 'null';
  }
}



  

  GetEmployee() {
    this.employee.getEmployee().subscribe((res: any) => {
      this.GetEmployeeData = res.httpResponse;
      console.log('employee data in navbar', this.GetEmployeeData);
    });
  }

  SendEmployeIdToService() {
    this.frm.valueChanges.subscribe(value => {
      this.EmployeFlag.setSelectedEmployee(value.SelectedEmploye); // Set the selected employee in the service
      console.log('form value', value);
    });
  }


//   checkAndRetrieveEmployee(): void {
//     if (this.Editflag) {
//       console.warn('welcome in true');
//       this.retrieveSelectedEmployee();
//       // Ensure the form control value is set after retrieving the selected employee
//       this.frm.patchValue({ SelectedEmploye: this.currentId });
//       // Manually set the selected employee in the service
//       this.EmployeFlag.setSelectedEmployee(this.currentId);
//     }
// }

  async personal() {
    this.vcr.clear();
    const { PersonalTabComponent } = await import('./personal-tab/personal-tab.component');
    this.vcr.createComponent(
      this.resolver.resolveComponentFactory(PersonalTabComponent)
    );
  }

  async financial() {
    this.vcr.clear();
    const { FinancialTabComponent } = await import('./financial-tab/financial-tab.component');
    this.vcr.createComponent(
      this.resolver.resolveComponentFactory(FinancialTabComponent)
    );
  }

  
  isActive(idx: number): boolean {
    const activeIndex = this.getActiveTabIndex; // Implement this method to get the currently active tab index
    return idx === activeIndex;
  }
 
  isDefaultOn(idx: number): boolean {
    return idx === 0; // Set the first tab (Personal) as default
  }
  
  navItemClick(idx: number) {
    const navList = document.querySelectorAll('.nav li');
    navList.forEach((item, index) => {
      item.classList.remove('on');
      if (index === idx) {
        item.classList.add('on');
      }
    });
  } 
  
  snavItemClick(tabNum: string, idx: number) {
    const snavList = document.querySelectorAll('.snav');
    const contList = document.querySelectorAll('.cont');
    snavList.forEach((item, index) => {
      const snavTabNum = item.classList[1].split('s')[1];
      if (snavTabNum === tabNum) {
        const snavItems = item.querySelectorAll('li');
        snavItems.forEach((snavItem, snavIndex) => {
          snavItem.classList.remove('on');
          if (snavIndex === idx) {
            snavItem.classList.add('on');
          }
        });
      }
    });
    contList.forEach((item, index) => {
      const contTabNum = item.classList[0].split('cont')[1].split('_')[0];
      const contIdx = item.classList[0].split('cont')[1].split('_')[1];
      if (contTabNum === tabNum && contIdx === (idx + 1).toString()) {
        item.classList.add('show');
      } else {
        item.classList.remove('show');
      }
    });
  }

}
