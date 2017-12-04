import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { SampleListComponent } from './sample-list.component';

import { AuthGuard }                from './../../services/auth-guard.service';

const samplesRoutes: Routes = [
	{
    path: '',
    component: SampleListComponent,
    canActivate: [AuthGuard],
  },
];

@NgModule({
	imports: [
		RouterModule.forChild(samplesRoutes)
	],
	exports: [
		RouterModule
	]
})
export class SamplesRoutingModule { }
