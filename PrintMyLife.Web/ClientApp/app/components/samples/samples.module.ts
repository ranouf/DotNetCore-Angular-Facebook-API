import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthGuard }            from './../../services/auth-guard.service';
import { AuthService }          from './../../services/auth.service';

import { SampleListComponent } from './sample-list.component';
import { SamplesService } from './../../services/api.services';
import { SamplesRoutingModule } from './samples-routing.module';

import { MatCardModule } from '@angular/material';
import { CovalentMediaModule } from '@covalent/core';

@NgModule({
	imports: [
		CommonModule,
		FormsModule,
		SamplesRoutingModule,
		MatCardModule,
		CovalentMediaModule,
	],
	declarations: [
		SampleListComponent,
	],
	providers: [
    SamplesService,
    AuthService,
    AuthGuard
  ]
})
export class SamplesModule { }
