import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { Rev1SignupComponent } from './rev1-signup.component';
import { Rev1EnterComponent } from './rev1-enter.component';


export const rev1Routes: Routes = [
    { path: '', pathMatch: 'full', redirectTo: 'signup' },
    { path: 'signup', component: Rev1SignupComponent, outlet: 'rev1Outlet' },
    { path: 'enter', component: Rev1EnterComponent, outlet: 'rev1Outlet' },

];
@NgModule({
    imports: [RouterModule.forChild(rev1Routes)],
    exports: [RouterModule]
})
export class Rev1RoutingModule {
    static components = [Rev1SignupComponent, Rev1EnterComponent ];
}


