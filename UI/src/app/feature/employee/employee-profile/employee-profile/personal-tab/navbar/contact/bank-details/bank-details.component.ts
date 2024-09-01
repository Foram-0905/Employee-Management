import { Component , ElementRef, OnInit,ViewChild} from '@angular/core';
import { BankDetails, SaveBankDetails } from '../../../../../../../../shared/models/bank-details';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { first } from 'rxjs';
import { BankDetailsService } from '../../../../../../../../shared/services/bankdetails.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { TranslateService } from '@ngx-translate/core';
import { employee } from '../../../../../../../../shared/models/employee';
import { EmployeeService } from '../../../../../../../../shared/services/employee.service';
import { ActionEnum } from '../../../../../../../../shared/constant/enum.const';

@Component({
  selector: 'app-bank-details',
  templateUrl: './bank-details.component.html',
  styleUrl: './bank-details.component.scss'
})
export class BankDetailsComponent implements OnInit {
  dataRowSource!: any[];
  bankdetails: BankDetails[]=[];
  savebankdetails = {} as SaveBankDetails;
  // private gridApi!: GridApi;
  // sortModel = {} as sortModel;
  editId: string = '';
  @ViewChild('addEditModal') addEditModal!: any;
  @ViewChild('addBtn') addBtn!: ElementRef;
  @ViewChild('closeButton') closeButton!: ElementRef;
  totalItems: any;
  // private helper = new commonHelper();
  showDeleteModal: boolean = false;
  constructor(private http: HttpClient,
    private translateService: TranslateService,
    private BankDetailsService: BankDetailsService,
    private EmployeeService : EmployeeService,
    private toastr: ToastrService,
   ) {
    this.translateService.setDefaultLang('en');
    this.translateService.use(localStorage.getItem('lang') || 'en')
  }
  ngOnInit(): void {
  
  }
  getBankdetails(): void {
    this.BankDetailsService.GetAllBankDetails()
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          // this.toastr.success(resp.message);
          // var allSLGGroup = resp;
          this.getBankdetails = resp.httpResponse;
        },
        error: (err: any) => {
          this.toastr.error(err);
        },
      });
  }
  SaveBankDetails() {
    if (!this.savebankdetails.id) {
      this.savebankdetails.action = ActionEnum.Insert;
    } else {
      this.savebankdetails.action = ActionEnum.Update;
    }

    if (this.ValidateModalData()) {
      this.BankDetailsService
        .SaveBankDetails(this.savebankdetails)
        .pipe(first())
        .subscribe({
          next: (resp: any) => {
            this.toastr.success(resp.message);
            this.getBankdetails();
            // this.closeModal();
            this.savebankdetails = {} as SaveBankDetails;
          },
          error: (err: any) => {
            this.toastr.error(err);
          },
        });
    }
  }
  ValidateModalData(): boolean {
    if (!this.savebankdetails.accountHolder) {
      this.toastr.error('accountHolder name  required');
      return false;
    }
    if (!this.savebankdetails.accountNumber) {
      this.toastr.error('accountNumber name required');
      return false;
    }
    if (!this.savebankdetails.ifscCode) {
      this.toastr.error('ifscCode name required');
      return false;
    }
    if (!this.savebankdetails.bankName) {
      this.toastr.error('bankName name required');
      return false;
    }
    if (!this.savebankdetails.employee) {
      this.toastr.error('employee name required');
      return false;
    }
    return true;
  }
  // closeModal() {
  //   console.log('Close modal');
  //   this.savebankdetails = {} as SaveBankDetails;

  //   if (this.closeButton) {

  //     this.closeButton.nativeElement.click();
  //   }
  }

// }
