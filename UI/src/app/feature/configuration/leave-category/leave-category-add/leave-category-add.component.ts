import { Component } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-leave-category-add',
  templateUrl: './leave-category-add.component.html',
  styleUrl: './leave-category-add.component.scss'
})
export class LeaveCategoryAddComponent {
  constructor( private translateService: TranslateService) {
    this.translateService.setDefaultLang('en');
    this.translateService.use(localStorage.getItem('lang') || 'en')
  }
}
