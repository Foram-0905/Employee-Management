import { Component, Input, OnInit } from '@angular/core';
import { DesignationModule } from '../designation.module';
import { ManageDesignation, Savedesignation } from '../../../../shared/models/manage-designation';
import { DesignationService } from '../../../../shared/services/designation.service';
import { first } from 'rxjs';
import { ActionEnum } from '../../../../shared/constant/enum.const';
import { TranslateService } from '@ngx-translate/core';


@Component({
  selector: 'app-designation-add',
  templateUrl:'./designation-add.component.html',
  styleUrl: './designation-add.component.scss'
})
export class DesignationAddComponent implements OnInit{
  slgGroup:any;
  Designation = {} as Savedesignation;

  @Input() Id: string = '';

  constructor(private designtion:DesignationService,private translateService:TranslateService) {
    this.slgGroup=[{Id:"3fa85f64-5717-4562-b3fc-2c963f66afa6",value:"SLG1"},{Id:"3fa85f64-5717-4562-b3fc-2c963f66afa6",value:"SLG2"},{Id:"3fa85f64-5717-4562-b3fc-2c963f66afa6",value:"SLG3"},{Id:"3fa85f64-5717-4562-b3fc-2c963f66afa6",value:"SLG4"}];
    this.translateService.setDefaultLang('en');
    this.translateService.use(localStorage.getItem('lang') || 'en')
   }

  ngOnInit(): void {
    // console.log("Id," , this.Id)
  }

  SaveDesignation(){
    if(!this.Designation.id){
      this.Designation.action=ActionEnum.Insert;
    }
    else{
      this.Designation.action=ActionEnum.Update;
    }

    if(!this.Designation.initialStatus||!this.Designation.designation||!this.Designation.shortWord ||!this.Designation.displaySequence)
    {
      return;
    }

    this.designtion.saveDesignation(this.Designation).pipe(first()).subscribe({
      next:(resp:any)=>{
        alert(resp.message);
        this.Designation = {} as Savedesignation;
      },
      error:(err:any)=>{
        // console.log(err);
        alert(err);
      }
    });
  }

  onSelect(value: any) {
    this.Designation.initialStatus = value.target.value;
  }
}
