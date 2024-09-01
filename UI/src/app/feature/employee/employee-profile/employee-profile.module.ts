import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { EmployeeProfileRoutingModule } from './employee-profile-routing.module';
import { EmployeeProfileComponent } from './employee-profile/employee-profile.component';
import { PersonalTabComponent } from './employee-profile/personal-tab/personal-tab.component';
import { FinancialTabComponent } from './employee-profile/financial-tab/financial-tab.component';
import { NavbarComponent } from './employee-profile/personal-tab/navbar/navbar.component';
import { FNavbarComponent } from './employee-profile/financial-tab/navbar/navbar.component';
import { PersonalComponent } from './employee-profile/personal-tab/navbar/personal/personal.component';
import { ContactComponent } from './employee-profile/personal-tab/navbar/contact/contact.component';
import { IdentityCardComponent } from './employee-profile/personal-tab/navbar/identity-card/identity-card.component';
import { AssetsComponent } from './employee-profile/personal-tab/navbar/assets/assets.component';
import { EducationComponent } from './employee-profile/personal-tab/navbar/education/education.component';
import { JobComponent } from './employee-profile/personal-tab/navbar/job/job.component';
import { DocumentsComponent } from './employee-profile/personal-tab/navbar/documents/documents.component';
import { GeneralComponent } from './employee-profile/personal-tab/navbar/personal/general/general.component';
import { ChildrenInformationComponent } from './employee-profile/personal-tab/navbar/personal/children-information/children-information.component';
import { LanguageCompetenceComponent } from './employee-profile/personal-tab/navbar/personal/language-competence/language-competence.component';
import { JobTitleRoleComponent } from './employee-profile/personal-tab/navbar/personal/job-title-role/job-title-role.component';
import { LeadershipComponent } from './employee-profile/personal-tab/navbar/personal/leadership/leadership.component';
import { ProbationComponent } from './employee-profile/personal-tab/navbar/personal/probation/probation.component';
import { EndOfEmployementComponent } from './employee-profile/personal-tab/navbar/personal/end-of-employement/end-of-employement.component';
import { TerminationComponent } from './employee-profile/personal-tab/navbar/personal/termination/termination.component';
import { ContactHomeAddressComponent } from './employee-profile/personal-tab/navbar/contact/contact-home-address/contact-home-address.component';
import { WorkLocationComponent } from './employee-profile/personal-tab/navbar/contact/work-location/work-location.component';
import { BankDetailsComponent } from './employee-profile/personal-tab/navbar/contact/bank-details/bank-details.component';
import { PassportComponent } from './employee-profile/personal-tab/navbar/identity-card/passport/passport.component';
import { VisaComponent } from './employee-profile/personal-tab/navbar/identity-card/visa/visa.component';
import { BlueCardComponent } from './employee-profile/personal-tab/navbar/identity-card/blue-card/blue-card.component';
import { OtherWorkPermitsComponent } from './employee-profile/personal-tab/navbar/identity-card/other-work-permits/other-work-permits.component';
import { AssignmentComponent } from './employee-profile/personal-tab/navbar/assets/assignment/assignment.component';
import { AssignmentListComponent } from './employee-profile/personal-tab/navbar/assets/assignment-list/assignment-list.component';
import { EducationListComponent } from './employee-profile/personal-tab/navbar/education/education-list/education-list.component';
import { Education1Component } from './employee-profile/personal-tab/navbar/education/education/education.component';
import { JobHistoryComponent } from './employee-profile/personal-tab/navbar/job/job-history/job-history.component';
import { JobListComponent } from './employee-profile/personal-tab/navbar/job/job-list/job-list.component';
import { DocumentsListComponent } from './employee-profile/personal-tab/navbar/documents/documents-list/documents-list.component';
import { SalaryComponent } from './employee-profile/financial-tab/navbar/salary/salary.component';
import { BonusComponent } from './employee-profile/financial-tab/navbar/bonus/bonus.component';
import { BonusComponent1 } from './employee-profile/financial-tab/navbar/bonus/bonus/bonus.component';
import { ConsultantRateComponent } from './employee-profile/financial-tab/navbar/consultant-rate/consultant-rate.component';
import { SalaryOverviewComponent } from './employee-profile/financial-tab/navbar/salary/salary-overview/salary-overview.component';
import { PaymentLastMonthComponent } from './employee-profile/financial-tab/navbar/salary/payment-last-month/payment-last-month.component';
import { PaymentLaterLastMonthComponent } from './employee-profile/financial-tab/navbar/salary/payment-later-last-month/payment-later-last-month.component';
import { AddingNewRatesComponent } from './employee-profile/financial-tab/navbar/consultant-rate/adding-new-rates/adding-new-rates.component';
import { RateHistoryComponent } from './employee-profile/financial-tab/navbar/consultant-rate/rate-history/rate-history.component';
import { SharedModule } from '../../../shared/shared.module';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { AgGridModule } from 'ag-grid-angular';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {provideNativeDateAdapter} from '@angular/material/core';
import {MatDatepickerModule} from '@angular/material/datepicker';
import {MatFormFieldModule} from '@angular/material/form-field';
import { NgSelectModule } from '@ng-select/ng-select';
// import {AutocompleteLibModule} from 'angular-ng-autocomplete';


export function HttpLoaderFactory(http:HttpClient){
  return new TranslateHttpLoader(http,"./assets/i18n/",".json");
}

@NgModule({
  declarations: [
    EmployeeProfileComponent,
    PersonalTabComponent,
    FinancialTabComponent,
    NavbarComponent,
    FNavbarComponent,
    PersonalComponent,
    ContactComponent,
    IdentityCardComponent,
    AssetsComponent,
    EducationComponent,
    Education1Component,
    JobComponent,
    DocumentsComponent,
    GeneralComponent,
    ChildrenInformationComponent,
    LanguageCompetenceComponent,
    JobTitleRoleComponent,
    LeadershipComponent,
    ProbationComponent,
    EndOfEmployementComponent,
    TerminationComponent,
    ContactHomeAddressComponent,
    WorkLocationComponent,
    BankDetailsComponent,
    // PassportComponent,
    VisaComponent,
    BlueCardComponent,
    OtherWorkPermitsComponent,
    AssignmentComponent,
    AssignmentListComponent,
    EducationListComponent,
    JobHistoryComponent,
    JobListComponent,
    DocumentsListComponent,
    SalaryComponent,
    BonusComponent,
    BonusComponent1,
    ConsultantRateComponent,
    SalaryOverviewComponent,
    PaymentLastMonthComponent,
    PaymentLaterLastMonthComponent,
    AddingNewRatesComponent,
    RateHistoryComponent,
    
    
  ],
  imports: [
    CommonModule,
    EmployeeProfileRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    NgSelectModule,
    SharedModule,

    SharedModule,MatFormFieldModule,MatDatepickerModule,
    TranslateModule.forRoot(
      {
      loader:{
        provide:TranslateLoader,
        useFactory:HttpLoaderFactory,
        deps:[HttpClient]
      }
    }
    )
  ],
  providers: [provideNativeDateAdapter()],
})
export class EmployeeProfileModule { }
