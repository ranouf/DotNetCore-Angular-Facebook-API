import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { MatInputModule, MatCardModule, MatButtonModule } from '@angular/material';
import { CovalentLayoutModule, CovalentMessageModule, CovalentSearchModule, CovalentMediaModule } from '@covalent/core';

import { LoginComponent } from './login.component';
import { RegisterComponent } from './register.component';
import { AuthenticationRoutingModule } from './authentication-routing.module';

@NgModule({
	imports: [
		CommonModule,
    FormsModule,
		MatInputModule,
		MatCardModule,
		MatButtonModule,
		CovalentLayoutModule,
		CovalentMessageModule,
		CovalentSearchModule,
		CovalentMediaModule,
		AuthenticationRoutingModule
	],
	declarations: [
		LoginComponent,
    RegisterComponent,
	],
	providers: []
})
export class AuthenticationModule { }
