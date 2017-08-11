import { RouterModule, Routes } from '@angular/router';
import { CampaignsComponent } from './coreg/campaigns.component';


const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: '/campaigns' },
  {
    path: 'campaigns', component: CampaignsComponent,
  }

];

export const routing = RouterModule.forRoot(routes);
