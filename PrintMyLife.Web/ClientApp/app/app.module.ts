import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { Router } from '@angular/router';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';

import { AuthenticationModule } from './components/authentication/authentication.module';
import { AuthenticationService } from './services/api.services';
import { AuthService } from './services/auth.service';
import { DashboardModule } from './components/dashboard/dashboard.module';
import { SamplesModule } from './components/samples/samples.module';
import { MATERIAL_COMPATIBILITY_MODE } from '@angular/material';

import * as _fromAngularMaterial from '@angular/material';
import * as _fromCovalent from '@covalent/core';

@NgModule({
	imports: [
		BrowserModule,
		FormsModule,
		HttpModule,
		BrowserAnimationsModule,

		_fromAngularMaterial.MatIconModule,
		_fromAngularMaterial.MatButtonModule,
		_fromAngularMaterial.MatListModule,
		_fromAngularMaterial.MatTooltipModule,
		_fromCovalent.CovalentLayoutModule,
		_fromCovalent.CovalentMediaModule,

		//PrintMyLife
		AppRoutingModule,
		AuthenticationModule,
		DashboardModule,
		SamplesModule,
	],
	declarations: [
		AppComponent
	],
	providers: [
		AuthenticationService,
		AuthService,
		{ provide: MATERIAL_COMPATIBILITY_MODE, useValue: true },
	],
	bootstrap: [AppComponent]
})
export class AppModule {
	// Diagnostic only: inspect router configuration
	constructor(router: Router) {
		//console.log('Routes: ', JSON.stringify(router.config, undefined, 2));
	}
}
