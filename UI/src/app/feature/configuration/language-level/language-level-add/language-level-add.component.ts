import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { LanguageLevelService } from '../../../../shared/services/language-level.service';
import { LanguageLevel } from '../../../../shared/models/language-level';
import { ActionEnum } from '../../../../shared/constant/enum.const';
import { first } from 'rxjs';

@Component({
  selector: 'app-language-level-add',
  templateUrl:'./language-level-add.component.html',

  styleUrl: './language-level-add.component.scss'
})
export class LanguageLevelAddComponent implements OnInit {
  lang:string ='';
  Language: LanguageLevel ={} as LanguageLevel;

  constructor(private translateService:TranslateService,private LanguageLevelService:LanguageLevelService){
    this.translateService.setDefaultLang('en');
    this.translateService.use(localStorage.getItem('lang') || 'en')
  }

 
  ngOnInit(): void {
    this.lang = localStorage.getItem('lang') || 'en';
  }

  Savelanguagelevel(){
    if(!this.Language.id){
      this.Language.action=ActionEnum.Insert;
    }else{
      this.Language.action=ActionEnum.Update;
    }
    if (!this.Language.level){
      return;
    }
    this.LanguageLevelService.SaveLanguagelevel(this.Language).pipe(first()).subscribe({
      next:(resp:any)=>{
        alert(resp.message);
        this.Language={} as LanguageLevel;
      },
      error:(err:any)=>{
        // console.log(err);
        alert(err);
      }
      
    });
  }
}
