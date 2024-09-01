import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { SlgGroupService } from '../../../../shared/services/slg-group.service';
import { SLGGroup } from '../../../../shared/models/slg-group';
import { ActionEnum } from '../../../../shared/constant/enum.const';
import { first } from 'rxjs';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-slg-add',
  templateUrl: './slg-add.component.html',
  styleUrls: ['./slg-add.component.scss']
})
export class SlgAddComponent implements OnInit {
  lang: string = '';
  slgGroups: SLGGroup = {} as SLGGroup; // Ensure slgGroups is properly defined and initialized
  @ViewChild('closeButton') closeButton!: ElementRef;

  constructor(
    private translateService: TranslateService,
    private slgGroupService: SlgGroupService,
    private toastr: ToastrService
  ) {
    this.translateService.setDefaultLang('gr');
    this.translateService.use(localStorage.getItem('lang') || 'gr');
  }

  ngOnInit(): void {
    this.lang = localStorage.getItem('lang') || 'gr';


  }

 SaveSLGgroup () {

    // console.log('SLG Save Clicked');

    if (!this.slgGroups.id) {
      this.slgGroups.action = ActionEnum.Insert;
    } else {
      this.slgGroups.action = ActionEnum.Update;
    }

    if (this.ValidateModalData()) {
      this.slgGroupService.SaveSlggroup(this.slgGroups).pipe(first()).subscribe({
        next: (resp: any) => {
         this.toastr.success(resp.message);
          this.slgGroups = {} as SLGGroup;
          this.closeModal();

        },
        error: (err: any) => {

          console.log(err);
          this.toastr.error("save SLG" , err.message);
        }

      });
    }

  }

  ValidateModalData(): boolean {
    if (!this.slgGroups.initialStatus) {
      this.toastr.error("Initial SLG status required");
      return false;
    }
    if (!this.slgGroups.statusName ) {
      this.toastr.error("SLG status display name required");
      return false;
    }
    if ( !this.slgGroups.statusSequence) {
      this.toastr.error("Display sequence required");
      return false;
    }
    if (!this.slgGroups.relevantExperience ) {
      this.toastr.error("Relevant work eperience required");
      return false;
    }
    return true
  }

  closeModal() {
    // Check if the close button reference exists
    if (this.closeButton) {
      // Access the native element and trigger a click event
      this.closeButton.nativeElement.click();
    }
  }
}
