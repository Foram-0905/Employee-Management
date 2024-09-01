import { AfterViewInit, Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges, ViewChild } from '@angular/core';
import { AgGridAngular } from 'ag-grid-angular';
import { GridOptions, GridApi, SizeColumnsToContentStrategy, SizeColumnsToFitGridStrategy, ModuleRegistry, IServerSideDatasource, SizeColumnsToFitProvidedWidthStrategy, IDatasource, RowModelType, ColDef } from 'ag-grid-community';
import { ActionButtonsRendererComponent } from './actionButtonsRendererComponent.component';
import { pageChange, RequestWithFilterAndSort, sortModel } from '../../models/FilterRequset';
import { TranslateService } from '@ngx-translate/core';
@Component({
  selector: 'app-beon-grid',
  templateUrl: './beon-grid.component.html',
  styleUrls: ['./beon-grid.component.scss']
})
export class BeonGridComponent implements OnInit, OnChanges, AfterViewInit {
  @Input() dataRowSource: any[] = [];
  @Input() columnDefs: any[] = [];
  @Input() showPagination: boolean = true;
  @Input() showEditButton: boolean = false;
  @Input() showDeleteButton: boolean = false;
  @Input() showDownloadButton: boolean = false;
  @Input() showCheckboxButton: boolean = false;
  @Input() showRejectButton: boolean = false;
  @Input() showApproveButton: boolean = false;
  // @Input() showAppoveButton: boolean = false;
  @Input() totalItems: any;
  @Input() defaultColDef: ColDef = {
    filter: true, // Set filter to true by default
    sortable: true // Set sortable to true by default
  };
  @Input() totalPages: any;

  @Output() ongridReady = new EventEmitter<AgGridAngular>();
  @Output() onEdit = new EventEmitter<any>();
  @Output() onDelete = new EventEmitter<any>();
  @Output() onDownload = new EventEmitter<any>();
  @Output() onCheckbox = new EventEmitter<any>();
  @Output() getDataRowsourse = new EventEmitter<any>();
  @Output() onApproveClick = new EventEmitter<any>();
  @Output() onRejectClick = new EventEmitter<any>();


  @ViewChild('agGrid') agGrid!: AgGridAngular;

  agGridTheme: string = "material";
  gridOptions: GridOptions = {};
  private agGridParams = {} as AgGridAngular;
  pageChange = {} as pageChange;
  autoSizeStrategy: SizeColumnsToFitGridStrategy | SizeColumnsToFitProvidedWidthStrategy | SizeColumnsToContentStrategy = {
    type: "fitGridWidth",
    defaultMinWidth: 120,
    columnLimits: [
      {
        colId: "Action",
        maxWidth: 150,
        minWidth: 142
      },
    ],
  };

  private gridApi!: GridApi;
  frameworkComponents: any;
  gridColumnApi: any;
  public noRowsTemplate: any;
  public loadingTemplate: any;
  pagenumber: number = 1;
  pageSizes: any = 5;
  requestWithFilterAndPage = {} as RequestWithFilterAndSort;
  sortModel = {} as sortModel;
  displayedPageNumbers: number[] = [];
  lang!: string;

  constructor(private translateService: TranslateService,) {
    this.requestWithFilterAndPage.pageNumber = 1;
    this.requestWithFilterAndPage.pageSize = 5;
    this.translateService.setDefaultLang('en');
    this.translateService.use(localStorage.getItem('lang') || 'en')
    this.noRowsTemplate = `No record found`;
    this.frameworkComponents = {
      buttonRenderer: ActionButtonsRendererComponent,
    };
  }

  ngOnInit(): void {
    this.lang = localStorage.getItem('lang') || 'en';
    this.gridOptions = {
      defaultColDef: this.defaultColDef,
      rowSelection: 'multiple' // Assign the default column definition to gridOptions
    };
    // console.warn('page', this.totalPages);

    if (this.totalPages && this.totalPages.length > 0) {
      this.updatePageNumbers();
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['totalPages'] && changes['totalPages'].currentValue) {
      this.updatePageNumbers();
    }

    if (changes['showEditButton'] || changes['showDeleteButton'] || changes['showDownloadButton'] || changes['showApproveButton'] || changes['showRejectButton']) {
      this.updateColumnDefs();
    }
  }

  ngAfterViewInit(): void {
    this.updateColumnDefs();
  }

  onGridReady(params: any): void {
    this.agGridParams = params;
    this.gridApi = params.api;
    this.gridColumnApi = params.columnApi;
    this.ongridReady.emit(params);
  }

  onSelectionChanged(event: any): void {
    const selectedRows = this.gridApi.getSelectedRows();
  }

  private updateColumnDefs(): void {
    // // debugger;
    if (!this.columnDefs) {
      this.columnDefs = [];
    }
    if (this.showApproveButton || this.showRejectButton || this.showEditButton || this.showDownloadButton || this.showDeleteButton) {
      let IsAction = false;
      this.columnDefs.forEach((item: any) => {
        if (item.headerName === 'Action') {
          IsAction = true;
        }
      });
      if (!IsAction) {
        this.columnDefs.push({
          headerName: this.translateService.instant('i18n.Common.Action'),
          colId: "Action",
          cellRenderer: 'buttonRenderer',
          cellRendererParams: {
            showDownloadButton: this.showDownloadButton,
            showEditButton: this.showEditButton,
            showDeleteButton: this.showDeleteButton,
            showCheckboxButton: this.showCheckboxButton,
            showRejectButton: this.showRejectButton,
            showApproveButton: this.showApproveButton,
            onDownloadClick: this.onDownloadButtonClick.bind(this),
            onClick: this.onEditButtonClick.bind(this),
            onDeleteClick: this.onDeleteButtonClick.bind(this),
            onCheckboxChange: this.onCheckboxChange.bind(this),
            onRejectClick: this.onRejectButtonClick.bind(this),
            onApproveClick: this.onApproveButtonClick.bind(this),
          },
          filter: false,
          sortable: false
        });
      }
    }

    // console.log("columnDefs",this.columnDefs);
  }

  onEditButtonClick(params: any): void {
    this.onEdit.emit(params.data);
  }

  onDownloadButtonClick(params: any): void {
    this.onDownload.emit(params.data);
  }

  onDeleteButtonClick(params: any): void {
    this.onDelete.emit(params.data);
  }

  onCheckboxChange(event: any): void {
    this.onCheckbox.emit(event);
  }

  onApproveButtonClick(params: any): void {
    this.onApproveClick.emit(params.data);
  }

  onRejectButtonClick(params: any): void {
    this.onRejectClick.emit(params.data);
  }

  onFilterChanged(): void {
    this.requestWithFilterAndPage.pageNumber = 1;
    this.pagenumber = 1;
    this.requestWithFilterAndPage.filterModel = this.gridApi.getFilterModel();
    this.getDatasourse();
  }

  onSortChanged(event: any): void {
    const sortModel = this.gridApi.getColumnState().find((s: any) => s.sort != null);
    if (sortModel) {
      this.sortModel.colId = sortModel.colId;
      this.sortModel.sortOrder = sortModel.sort;
    }
    this.requestWithFilterAndPage.sortModel = this.sortModel;
    this.getDatasourse();
  }

  onPageSize(event: any): void {
    this.pageSizes = event.target.value;
    this.requestWithFilterAndPage.pageNumber = 1;
    this.pagenumber = this.requestWithFilterAndPage.pageNumber;
    this.requestWithFilterAndPage.pageSize = this.pageSizes;
    this.getDatasourse();
  }

  updatePageNumbers(): void {
    const pageCount = 3; // Number of page numbers to display
    const totalPages = this.totalPages.length;

    let startPage: number;
    let endPage: number;

    if (totalPages <= pageCount) {
      // If the total number of pages is less than or equal to the pageCount, show all pages
      startPage = 1;
      endPage = totalPages;
    } else {
      // Otherwise, determine the start and end pages based on the current page number
      if (this.pagenumber <= 2) {
        startPage = 1;
        endPage = pageCount;
      } else if (this.pagenumber + 1 >= totalPages) {
        startPage = totalPages - pageCount + 1;
        endPage = totalPages;
      } else {
        startPage = this.pagenumber - 1;
        endPage = this.pagenumber + 1;
      }
    }

    // Update the displayedPageNumbers array with the calculated start and end pages
    this.displayedPageNumbers = Array.from({ length: endPage - startPage + 1 }, (_, i) => startPage + i);
  }

  navigateToPageNumbers(pageNumber: number): void {
    this.pagenumber = pageNumber;
    this.updatePageNumbers();
    this.requestWithFilterAndPage.pageNumber = this.pagenumber;
    this.getDatasourse();
  }

  firstPage(): void {
    this.pagenumber = 1;
    this.updatePageNumbers();
    this.requestWithFilterAndPage.pageNumber = this.pagenumber;
    this.getDatasourse();
  }

  prev(): void {
    if (this.pagenumber > 1) {
      this.pagenumber -= 1;
      this.updatePageNumbers();
      this.requestWithFilterAndPage.pageNumber = this.pagenumber;
      this.getDatasourse();
    }
  }

  next(): void {
    if (this.pagenumber < this.totalPages.length) {
      this.pagenumber += 1;
      this.updatePageNumbers();
      this.requestWithFilterAndPage.pageNumber = this.pagenumber;
      this.getDatasourse();
    }
  }

  lastPage(): void {
    this.pagenumber = this.totalPages.length;
    this.updatePageNumbers();
    this.requestWithFilterAndPage.pageNumber = this.pagenumber;
    this.getDatasourse();
  }

  getDatasourse(): void {
    // Add your logic to fetch data based on this.requestWithFilterAndPage
    this.getDataRowsourse.emit(this.requestWithFilterAndPage);
  }
  
}
