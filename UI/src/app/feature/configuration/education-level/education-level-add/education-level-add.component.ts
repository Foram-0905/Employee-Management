import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { EducationLevel } from '../../../../shared/models/education-level';

@Component({
  selector: 'app-education-level-add',
  templateUrl: './education-level-add.component.html',
  styleUrl: './education-level-add.component.scss'
})
export class EducationLevelAddComponent implements OnInit {

  lang:string ='';
  Education: EducationLevel ={} as EducationLevel;

  constructor(private translateService:TranslateService,){
    this.translateService.setDefaultLang('en');
    this.translateService.use(localStorage.getItem('lang') || 'en')
  }

 
  ngOnInit(): void {
    this.lang = localStorage.getItem('lang') || 'en';
  }


  Save_eductionlevel(){

  }
}
