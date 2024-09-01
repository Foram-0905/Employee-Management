import { Component ,OnInit} from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-assets-add',
  templateUrl: './assets-add.component.html',
  styleUrl: './assets-add.component.scss'
})
export class AssetsAddComponent implements OnInit{
  selectedFileName: string = '';
  lang:string="";
  constructor(private translateService: TranslateService) {
    this.translateService.setDefaultLang('en');
    this.translateService.use(localStorage.getItem('lang') || 'en')
  }

  ngOnInit(): void {
    // this.loadTranslations()
    this.lang = localStorage.getItem('lang') || 'en';

  }

  onFileSelected(event: any) {
    const file: File = event.target.files[0];
    if (file) {
      this.selectedFileName = file.name;
    } else {
      this.selectedFileName = '';
    }
  }
}
