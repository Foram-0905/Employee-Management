import { Component, OnInit } from '@angular/core';
import { LanguageCompetenceService } from '../../../../../../../../shared/services/LanguageCompetence.service';
import { LanguageLevelService } from '../../../../../../../../shared/services/language-level.service';
import { TranslateService } from '@ngx-translate/core';
import { first } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { LanguageCompetence } from '../../../../../../../../shared/models/language-competence';
import { ActionEnum } from '../../../../../../../../shared/constant/enum.const';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
@Component({
  selector: 'app-language-competence',
  templateUrl: './language-competence.component.html',
  styleUrl: './language-competence.component.scss'
})
export class LanguageCompetenceComponent  {
  dataRowSource!: any[];
  showDelete: boolean = true;
  AllLanguageCompetence:any;
  LanguageCompetence = {} as LanguageCompetence;
  languagelevedata:any;
  selectedFileName: string = '';
  employeeis='467f8713-963e-4db3-dc4d-08dc5ede353f';
  // employeeis='a9c04385-2fe2-4629-2e85-08dc5f8a63d4';

  // columnDefs = [

  //   // { headerName: 'Id', field: 'id',sortable: false, filter: true },
  //   { headerName: 'Other languages', field: 'name', sortable: false, filter: true },
  //   { headerName: 'Select Level', field: 'languageLevel.level', sortable: false, filter: true },
  //   { headerName: 'Languages Certificate', field: 'languagesCertificate', sortable: false, filter: true },
  //   // { headerName: 'Relevent Experience', field: 'relevantExperience', sortable: false, filter: true },


  // ];




  // constructor(private languageCompetence:LanguageCompetenceService,
  //   private languagelevel:LanguageLevelService,
  //   private translateService: TranslateService,
  //   private toastr: ToastrService,
    
  // ){
  //   this.translateService.setDefaultLang('en');
  //   this.translateService.use(localStorage.getItem('lang') || 'en');
   
  // }

  
  

  // ngOnInit(): void {
  //  this.GetLanguageCompetencesByEmployeeId()
  //  this.GetLanguageLevel()
  // }
  // GetLanguageCompetencesByEmployeeId(){
  //   this.languageCompetence
  //   .GetLanguageCompetenceByEmployeeid(this.employeeis)
  //   .pipe(first())
  //   .subscribe({
  //     next:(resp:any)=>{
  //       // this.toastr.success(resp.message);
  //       this.AllLanguageCompetence=resp;
  //       this.dataRowSource =this.AllLanguageCompetence.httpResponse;  
  //       console.warn("allLan",this.dataRowSource);
  //     },
  //     error: (err: any) => {
  //       this.toastr.error("Designation", err.message);
  //     },
  //   })
  // }

 
  
  // ValidateModalData(): boolean {
  //   if (!this.LanguageCompetence.name) {
  //     this.toastr.error('Select Language required');
  //     return false;
  //   }
  //   if (!this.LanguageCompetence.level) {
  //     this.toastr.error('Select Level required');
  //     return false;
  //   }
  //   if (!this.LanguageCompetence.languagesCertificate) {
  //     this.toastr.error('languagesCertificate required');
  //     return false;
  //   }
   
  //   return true;
  // }

  // SaveLanguageCompetence(){
  //   if(!this.LanguageCompetence.id){
  //     this.LanguageCompetence.employeeId =this.employeeis;
  //     this.LanguageCompetence.action = ActionEnum.Insert;
  //   }else{
  //     this.LanguageCompetence.action = ActionEnum.Update;
  //   }
  //   if (this.ValidateModalData()) {
  //     this.languageCompetence
  //       .SaveLanguageCompetence(this.LanguageCompetence)
  //       .pipe(first())
  //       .subscribe({
  //         next: (resp: any) => {
  //           this.toastr.success(resp.message);
            
          
  //           this.LanguageCompetence = {} as LanguageCompetence;
  //           this.GetLanguageCompetencesByEmployeeId();
  //         },
  //         error: (err: any) => {
  //           this.toastr.error("Save LanguageCompetence", err.message);
  //         },
  //       });
  //   }
  // }

  // onFileSelected(event: any) {
  //   const file: File = event.target.files[0];
  //   const reader = new FileReader();
  //   this.selectedFileName = file.name;
  //   reader.onload = (e: any) => {
  //     // Convert file to base64 and assign to ngModel
  //     this.LanguageCompetence.languagesCertificate = e.target.result;
  //   };
  
  //   reader.readAsDataURL(file);
  // }

  // setDefaultName(selectedLanguage: string) {
  //   if (selectedLanguage === 'English') {
  //     this.LanguageCompetence.name = 'English';
  //   } else if (selectedLanguage === 'Germany') {
  //     this.LanguageCompetence.name = 'Germany';
  //   }
  // }
  

  // onDeleteClick(currentRowData: any) {
   
  // }

}
