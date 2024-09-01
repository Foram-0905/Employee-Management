import { filterModel } from './../../../../shared/models/FilterRequset';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CalendarOptions } from '@fullcalendar/core';
import dayGridPlugin from '@fullcalendar/daygrid';
import { LeaveService } from '../../../../shared/services/leave.service';
import { first } from 'rxjs';
import interactionPlugin from '@fullcalendar/interaction';

@Component({
  selector: 'app-calender',
  templateUrl: './calender.component.html',
  styleUrl: './calender.component.scss'
})
export class CalenderComponent {
  // calendarOptions: CalendarOptions = {
  //   plugins: [dayGridPlugin, interactionPlugin],
  //   dateClick: (args: any) => this.dateClick(args),
  //   initialView: 'dayGridMonth',
  //   weekends: true,
  // };
  // employeeLeaves: any;
  leaves: any[] = [];
  selectedEmployee:any="All";
  filterModel:any;
  @Input() calendarOptions: any;
  @Input() activeTab: any;
  @Input() employeeList: any;

  @Output() onDateClick = new EventEmitter<any>();
  @Output() onSelectEmployee=new EventEmitter<any>();
  constructor(private leaveService: LeaveService) { }

  ngOnInit(): void {
    // this.setLeaveInCalendar();

  }
  dateClick(args: any) {
    console.log("daste", args);
    this.onDateClick.emit(args);
  }
  customizeEventContent(arg: any) {
    return arg.el.style.height = '5px';
  }

  handleEventMouseEnter(event:any) {

    // Handle mouse enter event
    // console.log('Mouse enter event:', event);
  }

  handleEventMouseLeave(event:any) {
    // Handle mouse leave event
    // console.log('Mouse leave event:', event);
  }


  changeEmployee(){
    let newFilterModel: filterModel = {
      filterType: "text",
      type: "contains",
      filter:this.selectedEmployee,
    };
    // Add the new filter model to the existingRequest filterModel object
    // this.filterModel["employeeId"] = newFilterModel;
    this.onSelectEmployee.emit(newFilterModel);

  }
  // GetAllLeaves() {
  //   this.leaveService
  //     .GetAllLeaves()
  //     .pipe(first())
  //     .subscribe((resp) => {
  //       this.employeeLeaves = resp.httpResponse;
  //      this.setLeaveInCalendar();
  //     });
  // }

  // setLeaveInCalendar() {
  //   this.employeeLeaves.forEach((element: any) => {
  //     let color: string;
  //     switch (element.leavetypeId.typeName) {
  //       case 'Sick Leave':
  //         color = "Red";
  //         break;
  //       case 'Annual Leave':
  //         color = "Green";
  //         break;
  //       case 'Special Leave':
  //         color = "#6b33e0";
  //         break;
  //       default:
  //         color = '#52575d';
  //         break;
  //     }

  //     const leave = {
  //       title: '',
  //       start: element.startDate,
  //       end: element.endDate,
  //       color: color
  //     };
  //     this.leaves.push(leave);
  //   });
  //   this.calendarOptions.events = this.leaves;

  //   console.log("asd", this.calendarOptions.events);

  // }
}
