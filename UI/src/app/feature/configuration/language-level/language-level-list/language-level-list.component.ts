import { Component,ElementRef, OnInit, ViewChild } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { first } from 'rxjs';
import { Router } from '@angular/router';
import { ActionEnum } from '../../../../shared/constant/enum.const';
import { LanguageLevelService } from '../../../../shared/services/language-level.service';
import { LanguageLevel } from '../../../../shared/models/language-level';
import { ToastrService } from 'ngx-toastr';
import { RequestWithFilterAndSort, sortModel } from '../../../../shared/models/FilterRequset';
import { totalPageArray } from '../../../../shared/helpers/common.helpers';
import { commonHelper } from '../../../../shared/helpers/common.helpers';
import { PermissionService } from '../../../../shared/services/permission.service';
import { NgxSpinnerService } from 'ngx-spinner';
@Component({
  selector: 'app-language-level-list',
  templateUrl:'./language-level-list.component.html',
  styleUrl:'./language-level-list.component.scss'
})
export class LanguageLevelListComponent implements OnInit {
 //***********  Configure AG-Grid  ****************/
 PageSize !: number;
 PageNumber!: number;
 LanguageLevel: LanguageLevel = {} as LanguageLevel;
 showDeleteModal: boolean = false;
 dataRowSource!: any[];
 @ViewChild('closeButton') closeButton!: ElementRef;
 @ViewChild('addBtn') addBtn!: ElementRef;
 requestWithFilterAndPage = {} as RequestWithFilterAndSort;
 sortModel = {} as sortModel;
 totalPages: number[] = [];
 totalItems: any;
 gridApi: any;
 searchText: any;
 private helper = new commonHelper();
 columnDefs:any[]=[];
//  columnDefs = [
//    { headerName: 'Level', field: 'level' }
//  ];
 showEdit: boolean = true;
 showDelete: boolean = true;
 currentId: any;
 allLanguageLevel: any;
 DeleteLanguageLevel: any;
  editId: any;
  lang: string = '';

 constructor(private languageservice:LanguageLevelService,
  private translateService: TranslateService,
   private router: Router,
   private toaster:ToastrService,
   private permission: PermissionService,
    private totalPageArray: totalPageArray,
    private spinner: NgxSpinnerService )
   {
    this.requestWithFilterAndPage.pageNumber = 1;
    this.requestWithFilterAndPage.pageSize = 5;
    this.sortModel.colId = "level";
    this.sortModel.sortOrder = "asc";
    this.requestWithFilterAndPage.sortModel = this.sortModel;
    this.requestWithFilterAndPage.filterModel = {};
    this.translateService.setDefaultLang('en');
    this.translateService.use(localStorage.getItem('lang') || 'en')
   }

   ngOnInit(): void {
    this.lang=localStorage.getItem('lang') || 'en';
    setTimeout(() => {
      this.columnDefs = [
        { headerName: this.translateEnumValue('i18n.configuration.languageLevelDetails.languagelevel'), field: 'level' }
      ];
    }, 50);
    this.gettFilterLanguageData();
  }
  translateEnumValue(enumValue: string): string {
    return this.translateService.instant(`${enumValue}`);

  }

  getAllLanguageData(){
    this.languageservice
    .GetAllLanguagelevel()
    .pipe(first())
    .subscribe({
      next: (resp: any) =>{
        this.allLanguageLevel=resp.httpResponse;
        // this.dataRowSource=this.allLanguageLevel
      },
      error: (err: any) => {
        this.toaster.error(err);
      },

    })
  }
  gettFilterLanguageData(){
    this.languageservice
    .getFiltersLanguagelevel
    (this.requestWithFilterAndPage)
    .pipe(first())
    .subscribe((resp)=>
    {
        // console.warn(resp,"filter");
      this.totalItems = resp.httpResponse.totalRecord;
      this.dataRowSource = resp.httpResponse.languagelevel;
      this.totalPages = this.totalPageArray.GetTotalPageArray(this.totalItems, this.requestWithFilterAndPage.pageSize);
    })
  }

  hasPermission(permission: any) {
    debugger
    return this.permission.hasPermission(permission);
  }

  onEditClick(currentRowData: any): void {
    this.editId = currentRowData.id;
    this.languageservice
      .GetLanguagelevelById(this.editId)
      .pipe(first())
      .subscribe((resp) => {
        this.allLanguageLevel = resp;
        this.LanguageLevel = this.allLanguageLevel.httpResponse;
        /// // debugger
        if (this.LanguageLevel) {
          if (this.addBtn) {
            this.addBtn.nativeElement.click();
          }
        }
      });
   }

   onDeleteClick(currentRowData: any) {
    this.editId = currentRowData.id;
    this.showDeleteModal = true;
    this.DeleteLanguageLevel = currentRowData.level;
  }

  cancelDelete() {
    this.showDeleteModal = false;
  }

  ValidateModalData(): boolean {
    if (!this.LanguageLevel.level) {
      this.toaster.error(this.translateService.instant("i18n.configuration.languageLevelDetails.Languagelevelrequired",(this.spinner.hide())));
      return false;
    }
   
    return true;
  }

  closeModal() {
    // Check if the close button reference exists
    if (this.closeButton) {
      // Access the native element and trigger a click event
      this.closeButton.nativeElement.click();
      this.LanguageLevel = {} as LanguageLevel;
    }
  }
  SaveLanguageLevel(){
    this.spinner.show();
    if (!this.LanguageLevel.id) {
      this.LanguageLevel.action = ActionEnum.Insert;
    } else {
      this.LanguageLevel.action = ActionEnum.Update;
    }

    if (this.ValidateModalData()) {
      this.languageservice
        .SaveLanguagelevel(this.LanguageLevel)
        .pipe(first())
        .subscribe({
          next: (resp: any) => {
            this.toaster.success(resp.message);
            this.gettFilterLanguageData();
            this.closeModal();
            this.spinner.hide();
            this.LanguageLevel = {} as LanguageLevel;
          },
          error: (err: any) => {
            this.toaster.error(err.message);
            this.spinner.hide();
          },
        });
    }
  }

  deleteItem() {
    this.spinner.show();
    this.languageservice
      .DeleteLanguagelevel(this.editId)
      .pipe(first())
      .subscribe({
        next: (resp: any) => {
          this.toaster.success(resp.message);
          this.showDeleteModal = false;
          this.getAllLanguageData();
          this.gettFilterLanguageData();
          this.spinner.hide();
        },
        error: (err: any) => {
          this.toaster.error(err);
          this.spinner.hide();
        },
      });
  }
 
  gridReady(params: any) {
    this.gridApi = params.api;
  }
  
  commonSearchWithinGrid() {
    this.gridApi.setFilterModel(null);
    // const gridColumnsToFileter: string[] = ['designation', 'initialStatus', 'displaySequence']
    const gridColumnsToFileter: string[] = ['level']
    this.helper.commonSearchWithinGrid(gridColumnsToFileter, this.searchText, this.requestWithFilterAndPage);
    // this.getDataRowsourse(this.requestWithFilterAndPage);
    this.gettFilterLanguageData();

  }
  onSearchInput(event: any) {
    const inputValue = event.target.value;
    if (inputValue.length==0 ||inputValue.length >= 4) {
      this.commonSearchWithinGrid();
    }
  }
 
  
  getDataRowsourse(event: RequestWithFilterAndSort) {
    // // debugger
    if (!event.sortModel) {
      event.sortModel = this.sortModel;
    }
    if (!event.filterModel) {
      event.filterModel = this.requestWithFilterAndPage.filterModel;
      event.filterConditionAndOr = this.requestWithFilterAndPage.filterConditionAndOr;
    }
    else {
      this.searchText = "";
    }
    this.requestWithFilterAndPage = event;

    this.gettFilterLanguageData();
  }

}
