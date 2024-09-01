import { Component } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-assets',
  templateUrl: './assets.component.html',
  styleUrl: './assets.component.scss'
})
export class AssetsComponent {

    constructor(private translateService: TranslateService,){

    }

    ngOnInit(): void {

      this.translateService.setDefaultLang('en');
      this.translateService.use(localStorage.getItem('lang') || 'en');
    }
}
