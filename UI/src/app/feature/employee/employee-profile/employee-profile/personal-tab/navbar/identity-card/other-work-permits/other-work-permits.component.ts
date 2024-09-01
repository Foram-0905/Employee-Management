import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';

@Component({
  selector: 'app-other-work-permits',
  templateUrl: './other-work-permits.component.html',
  styleUrl: './other-work-permits.component.scss'
})
export class OtherWorkPermitsComponent {

  // getAllWorkPermit() {
  //   this.workPermitService
  //     .getAllWorkPermit()
  //     .pipe(first())
  //     .subscribe({
  //       next: (resp: any) => {
  //         // this.toastr.success(resp.message);
  //         this.allWorkPermit = resp;
  //         console.log("data from server",this.allWorkPermit);
  //          this.dataRowSource = this.allWorkPermit.httpResponse;
  //       },
  //       error: (err: any) => {
  //         this.toastr.error("WorkPermit", err.message);
  //       },
  //     });
  // }
  
  // SaveWorkPermit() {
  //   if (!this.WorkPermit.id) {
  //     // this.WorkPermit.employeeId= localStorage.getItem('empId');
  //      this.WorkPermit.employeeId= "b40cedbf-ee5e-42c1-4269-08dc5edf1c51";
  
  //     this.WorkPermit.action = ActionEnum.Insert;
  //   } else {
  //     this.WorkPermit.action = ActionEnum.Update;
  //   }
  
  //   console.log("workpermit",this.WorkPermit);
    
  //   if (this.ValidateModalData()) {
  //     // this.WorkPermit.permitExpiryDate = new Date();
  //     this.workPermitService
  //       .saveWorkPermit(this.WorkPermit)
  //       .pipe(first())
  //       .subscribe({
  //         next: (resp: any) => {
  //           this.toastr.success(resp.message);
        
  //           // console.log('this.WorkPermit.permitExpiryDate', this.WorkPermit.permitExpiryDate);
            
  //           this.WorkPermit = {} as WorkPermit;
  //           this.getAllWorkPermit();
  //         },
  //         error: (err: any) => {
  //           this.toastr.error("Save Designation", err.message);
  //         },
  //       });
  //   }
  // }
  // ValidateModalData(): boolean {
  //   if (!this.WorkPermit.permitType) {
  //     this.toastr.error('permitType required');
  //     return false;
  //   }
  //   if (!this.WorkPermit.permitStartDate) {
  //     this.toastr.error('permitStartDate required');
  //     return false;
  //   }
  //   if (!this.WorkPermit.permitExpiryDate) {
  //     this.toastr.error('permitExpiryDate required');
  //     return false;
  //   }
  //   if (!this.WorkPermit.document) {
  //     this.toastr.error('document required');
  //     return false;
  //   }
  //   return true;
  // }

  // deleteItem() {
  //   this.workPermitService
  //     .deleteWorkPermit(this.editId)
  //     .pipe(first())
  //     .subscribe({
  //       next: (resp: any) => {
  //         this.toastr.success(resp.message);
  //         this.showDeleteModal = false;
  //         this.getAllWorkPermit();
  //       },
  //       error: (err: any) => {
  //         this.toastr.error(err);
  //       },
  //     });
  // }
}
