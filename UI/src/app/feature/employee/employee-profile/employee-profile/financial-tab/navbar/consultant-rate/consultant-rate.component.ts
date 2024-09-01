import { Component, OnInit } from '@angular/core';
import { PermissionService } from '../../../../../../../shared/services/permission.service';
import { CurrencyService } from '../../../../../../../shared/services/currency.service';
import { TranslateService } from '@ngx-translate/core';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { first } from 'rxjs';
import { Currency } from '../../../../../../../shared/models/currency';
import { consultant_rate } from '../../../../../../../shared/models/consultant_rate';
import { ActionEnum } from '../../../../../../../shared/constant/enum.const';
import { consultantRateService } from '../../../../../../../shared/services/consultantRate.service';
import { EmployeeFlagService } from '../../../../../../../shared/services/EmployeFlag.service';

@Component({
  selector: 'app-consultant-rate',
  templateUrl: './consultant-rate.component.html',
  styleUrl: './consultant-rate.component.scss',
})
export class ConsultantRateComponent implements OnInit {
  currenccy = {} as Currency;
  currency: any;
  consultantRate = {} as consultant_rate;
  SelectedEmployee!: string;

  constructor(
    private permission: PermissionService,

    private currencyService: CurrencyService,
    private ConsultantRateService: consultantRateService,
    private translateService: TranslateService,
    private router: Router,
    private modalService: NgbModal,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private EmployeFlage: EmployeeFlagService
  ) {
    this.translateService.setDefaultLang('en');
    this.translateService.use(localStorage.getItem('lang') || 'en');
  }

  ngOnInit(): void {
    this.getCurrency();
    this.EmployeFlage.selectedEmployee$.subscribe((employee) => {
      this.SelectedEmployee = employee;
    });
  }

  getCurrency() {
    this.currencyService
      .getCurrencies()
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.currency = resp.httpResponse;
          console.log('Currency', this.currency);
        },
        error: (err: any) => {
          this.toastr.error('salary currency', err.message);
        },
      });
  }
  onCurrencySelect(value: any) {
    // console.log("🚀 ~ SalaryComponent ~ onCurrencySelect ~ onCurrencySelect:")
    this.consultantRate.Currency = value.target.value;
    // this.salary.currency = this.currencytype;
  }

  SaveConsultantRate() {
    this.spinner.show();
    if (!this.consultantRate.id) {
      this.consultantRate.action = ActionEnum.Insert;
      // Use the selected employee ID for saving
      this.consultantRate.employeeId = this.SelectedEmployee;
    } else {
      this.consultantRate.action = ActionEnum.Update;
    }

    if (this.ValidateModalData()) {
      // Ensure transactionType is correctly set
      this.ConsultantRateService.saveConsultantRate(this.consultantRate)
        .pipe(first())
        .subscribe({
          next: (resp: any) => {
            this.toastr.success(resp.message);
            // this.consultantRate = {} as consultant_rate;
            // Fetch updated data for the selected employee
            this.consultantRate = {} as consultant_rate;

            this.spinner.hide();
          },
          error: (err: any) => {
            this.toastr.error('Save Consultant Rate', err.message);
            this.spinner.hide();
          },
        });
    } else {
      this.spinner.hide();
    }
  }

  ValidateModalData(): boolean {
    if (!this.consultantRate.Currency) {
      this.toastr.error(
        this.translateService.instant(
          'i18n.employeeProfile.finance-details.consultant-RateDetails.CurrencyRequired',
          this.spinner.hide()
        )
      );
      return false;
    }

    if (!this.consultantRate.PriceperDaynet) {
      this.toastr.error(
        this.translateService.instant(
          'i18n.employeeProfile.finance-details.consultant-RateDetails.Priceperdaynetrequired',
          this.spinner.hide()
        )
      );
      return false;
    }
    if (!this.consultantRate.PriceperHournet) {
      this.toastr.error(
        this.translateService.instant(
          'i18n.employeeProfile.finance-details.consultant-RateDetails.PriceperHournetrequired',
          this.spinner.hide()
        )
      );
      return false;
    }
    return true;
  }
}
