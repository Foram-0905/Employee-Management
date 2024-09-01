import { Component,OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
@Component({
  selector: 'app-public-holiday-add',
  templateUrl: './public-holiday-add.component.html',
  styleUrl: './public-holiday-add.component.scss'
})
export class PublicHolidayAddComponent implements OnInit{
  lang:string="";

  constructor(private translateService: TranslateService) {
    this.translateService.setDefaultLang('en');
    this.translateService.use(localStorage.getItem('lang') || 'en')
  }
  ngOnInit(): void {
    this.lang = localStorage.getItem('lang') || 'en';

  }
}
