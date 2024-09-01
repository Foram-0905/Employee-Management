import { Component, Input, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { first } from 'rxjs';
import { State } from '../../../../shared/models/state';
import { ActionEnum } from '../../../../shared/constant/enum.const';
import { StateRegionService } from '../../../../shared/services/state-region.service';

@Component({
  selector: 'app-state-region-add',
  templateUrl: './state-region-add.component.html',
  styleUrls: ['./state-region-add.component.scss']
})
export class StateRegionAddComponent implements OnInit {
  @Input() Countrydata!: any[];
  Country: { Id: string; value: string; }[];
  lang: string = '';
  State_region: State = {} as State;

  constructor(private translateService: TranslateService, private stateRegionService: StateRegionService) {
    this.translateService.setDefaultLang('en');
    this.translateService.use(localStorage.getItem('lang') || 'en');
    this.Country = [{ Id: "ce7c792d-93fe-45b7-9a84-910d31c366fd", value: "India" }];
    console.log(this.Countrydata);
  }

  ngOnInit(): void {
    this.lang = localStorage.getItem('lang') || 'en';
   
  }

  SaveState() {
    if (!this.State_region.id) {
      this.State_region.action = ActionEnum.Insert;
    } else {
      this.State_region.action = ActionEnum.Update;
    }
    if (!this.State_region.name || !this.State_region.countryId) {
      return;
    }
    this.stateRegionService.SaveState_Region(this.State_region).pipe(first()).subscribe({
      next: (resp: any) => {
        alert(resp.message);
        this.State_region = {} as State;
      },
      error: (err: any) => {
        console.log(err);
        alert(err);
      }
    });
  }
}
