import { Rev1Offer1Component } from './rev1/offers/rev1-offer1/rev1-offer1.component';
import { RouterModule, Routes } from '@angular/router';
import { Rev1Component } from './rev1/rev1.module';
import { rev1Routes, Rev1RoutingModule } from './rev1/rev1-routing.module';
import { Rev1SignupComponent } from './rev1/rev1-signup.component';
import { Rev1EnterComponent } from './rev1/rev1-enter.component';
import { Rev1RejectComponent } from './rev1/rev1-reject.component';

// const routes: Routes = [
//    { path: '', pathMatch: 'full', redirectTo: '/rev1' },
//    { path: 'rev1', component: CoregRev1Component, loadChildren: 'app/coreg/rev1-routing.module#Rev1RoutingModule' },
// ];

// const routes: Routes = [
//    { path: '', pathMatch: 'full', redirectTo: '/rev1' },
//    {
//        path: 'rev1',
//        component: Rev1Component, children :
//        [
//            { path: '', pathMatch: 'full', redirectTo: '/signup' },
//            { path: 'signup', component: Rev1SignupComponent, outlet: 'rev1Outlet' },
//            { path: 'enter', component: Rev1EnterComponent, outlet: 'rev1Outlet' },
//            { path: 'offers', component: Rev1OffersComponent, outlet: 'rev1Outlet' },
//        ]
//    },
// ];

const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: '/rev1signup' },
  {
    path: 'rev1signup', component: Rev1SignupComponent,

    // path: 'rev1',
    // component: Rev1Component, children:
    // [
    //    { path: '', pathMatch: 'full', redirectTo: '/signup' },
    //    { path: 'signup', component: Rev1SignupComponent, outlet: 'rev1Outlet' },
    //    { path: 'enter', component: Rev1EnterComponent, outlet: 'rev1Outlet' },
    //    { path: 'offers', component: Rev1OffersComponent, outlet: 'rev1Outlet' },
    // ]
  },
  {
    path: 'rev1enter', component: Rev1EnterComponent,
  },
  {
    path: 'offers1', component: Rev1Offer1Component,
  },
  {
    path: 'campaignComplete', component: Rev1RejectComponent,
  }
];

export const routing = RouterModule.forRoot(routes);
