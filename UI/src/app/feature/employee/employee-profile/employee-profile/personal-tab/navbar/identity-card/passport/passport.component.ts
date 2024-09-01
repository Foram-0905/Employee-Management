import { Component, ElementRef, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
// import { WorkPermitService } from '../../../../../../../../shared/services/workPermit.service';
import { first } from 'rxjs';
// import { WorkPermit } from '../../../../../../../../shared/models/work-permit';
import { ActionEnum } from '../../../../../../../../shared/constant/enum.const';

@Component({
  selector: 'app-passport',
  templateUrl: './passport.component.html',
  styleUrl: './passport.component.scss'
})
export class PassportComponent implements OnInit{
  dataRowSource!: any[];
  editId: string = '';
  showEdit: boolean = true;
  showDelete: boolean = true;
  totalPages: number[] = [];
  totalItems: any;
  searchText: any;
  allWorkPermit: any;
  // WorkPermit = {} as WorkPermit;
  selectedFileName: string = '';

  @ViewChild('addBtn') addBtn!: ElementRef;
  @ViewChild('closeButton') closeButton!: ElementRef;
  @ViewChild('addEditModal') addEditModal!: any;

  @Output() passportData: EventEmitter<any> = new EventEmitter<any>();

  constructor(
    // private workPermitService:WorkPermitService,
    private translateService: TranslateService,
    private router: Router,
    private modalService: NgbModal,
    private toastr: ToastrService,


  ) {
    
    this.translateService.setDefaultLang('en');
    this.translateService.use(localStorage.getItem('lang') || 'en');
  }
  ngOnInit(): void {
      // this.getAllWorkPermit();
  }


  
  
}
