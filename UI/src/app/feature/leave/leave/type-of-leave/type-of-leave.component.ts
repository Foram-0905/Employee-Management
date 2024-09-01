import { Component, Input, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { leaveTypeService } from '../../../../shared/services/leaveType.service';
import { first } from 'rxjs';
import { LeaveService } from '../../../../shared/services/leave.service';
import { typesOFLeaves } from '../../../../shared/models/employee-leave';
import { EmployeeService } from '../../../../shared/services/employee.service';

@Component({
  selector: 'app-type-of-leave',
  templateUrl: './type-of-leave.component.html',
  styleUrl: './type-of-leave.component.scss',
})
export class TypeOfLeaveComponent implements OnInit {
  allLeaveType: any[] = [];
  employeeLeaves: any;
  typesOFLeaves = {} as typesOFLeaves;
  @Input() displayLeaveType: any[]=[];
  @Input() activeTab: any;
  SelectedEmploy: any;

  constructor(
    private translateService: TranslateService,
    private AvailableLeave: EmployeeService,
    private leaveTypeService: leaveTypeService,
    private leaveService: LeaveService
  ) {
    // Retrieve the JSON string from localStorage
    let selectedEmployeeJson: string | null = localStorage.getItem(
      'CurrentEmployeeForNotification'
    );

    // Parse the JSON string back to a JavaScript object, or assign a default value if it's null
    let selectedEmployee = selectedEmployeeJson
      ? JSON.parse(selectedEmployeeJson)
      : null;

    // Declare a variable to hold the console message
    this.SelectedEmploy;

    if (selectedEmployee !== null) {
      // Remove quotes if selectedEmployee is a quoted string
      if (typeof selectedEmployee === 'string') {
        selectedEmployee = selectedEmployee.replace(/^"|"$/g, '');
      }
      // Now you can use the 'selectedEmployee' variable
      this.SelectedEmploy = selectedEmployee;
    } else {
      this.SelectedEmploy = 'No selected employee found in localStorage.';
    }

    // Output the message to the console
    console.log(this.SelectedEmploy);
  }

  ngOnInit(): void {
    //this.getLeaveType();
    this.GetAvailableLeave();
  }
  AvailableLeaveData: any[]=[];
  GetAvailableLeave() {
    this.AvailableLeave.GetAvailableLeave(this.SelectedEmploy)
      .pipe(first())
      .subscribe({
        next: (res) => {
          // Log the response to check its structure
          console.log('API response:', res);

          // Ensure the response is an array
          if (Array.isArray(res.httpResponse)) {
            this.AvailableLeaveData = res.httpResponse;
          } else {
            console.error('httpResponse is not an array:', res.httpResponse);
          }

          // console.warn('available', this.AvailableLeaveData);
          // console.warn('displayLeaveType', this.displayLeaveType);

          // Update displayLeaveType with leaveQuota values from AvailableLeaveData
          this.updateLeaveQuota();
        },
        error: (err) => {
          // console.error('Error fetching available leaves:', err);
        }
      });
  }

  updateLeaveQuota() {
    // Ensure both AvailableLeaveData and displayLeaveType are defined and are arrays
    if (Array.isArray(this.AvailableLeaveData) && Array.isArray(this.displayLeaveType)) {
      // Loop through displayLeaveType array
      this.displayLeaveType.forEach((displayLeave: any) => {
        // Find the corresponding item in AvailableLeaveData
        const matchingLeave = this.AvailableLeaveData.find((availableLeave: any) => availableLeave.leaveName === displayLeave.leaveType);
        
        // If a matching leave is found, update the leaveQuota
        if (matchingLeave) {
          displayLeave.leaveQuota = matchingLeave.leaveQuota;
        }
      });
      // console.warn('Updated displayLeaveType', this.displayLeaveType);
    } else {
      // console.error('AvailableLeaveData or displayLeaveType is not an array');
      // console.error('AvailableLeaveData:', this.AvailableLeaveData);
      // console.error('displayLeaveType:', this.displayLeaveType);
    }
  }

  getLeaveType() {
    this.leaveTypeService
      .getLeaves()
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.allLeaveType = resp.httpResponse;
          // console.warn('alllevaertype',this.allLeaveType);
        },
      });
  }
}
