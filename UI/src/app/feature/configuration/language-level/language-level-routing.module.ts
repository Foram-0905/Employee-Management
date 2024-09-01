import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LanguageLevelListComponent } from './language-level-list/language-level-list.component';

const routes: Routes = [
  {path:'languagelevel',component:LanguageLevelListComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class LanguageLevelRoutingModule { }
