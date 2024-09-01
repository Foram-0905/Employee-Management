import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class EmployeeFlagService {

    private currentId!: string;
    private editFlag!: boolean;
  
    constructor() { }
  
    setEmployeeParams(id: string, editFlag: boolean): void {
      this.currentId = id;
      this.editFlag = editFlag;
      console.log('serviceflag', this.currentId, this.editFlag);
    }

    getEmployeeParams(): any {
      return { id: this.currentId, Editflag: this.editFlag };
    }

    private selectedEmployeeSource = new BehaviorSubject<any>(null);
    selectedEmployee$ = this.selectedEmployeeSource.asObservable();
  
    setSelectedEmployee(employee: any) {
      this.selectedEmployeeSource.next(employee);
      // console.warn('service ke ander ',employee);
      
    }

}
