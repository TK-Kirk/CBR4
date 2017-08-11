import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';



import { CampaignsComponent } from './campaigns.component';

import { CoregService } from './services/coreg.service';

@NgModule({
  imports: [
    FormsModule,
    CommonModule
  ],
  declarations: [
    CampaignsComponent
  ],
  providers: [
    CoregService
  ],
  exports: [

  ]
})
export class CoregModule {
 
}
