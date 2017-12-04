import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MatInputModule, MatCardModule, MatButtonModule } from '@angular/material';
import { CovalentLayoutModule, CovalentMessageModule, CovalentSearchModule, CovalentMediaModule } from '@covalent/core';

import { DashboardComponent } from './dashboard.component';
import { DashboardRoutingModule } from './dashboard-routing.module';

@NgModule({
	imports: [
		CommonModule,
		MatInputModule,
		MatCardModule,
		MatButtonModule,
		CovalentLayoutModule,
		CovalentMessageModule,
		CovalentSearchModule,
		CovalentMediaModule,
		DashboardRoutingModule
	],
	declarations: [
		DashboardComponent,
	],
	providers: []
})
export class DashboardModule { }
