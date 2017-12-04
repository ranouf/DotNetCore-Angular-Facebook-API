import { Component, ViewEncapsulation, OnInit, ChangeDetectorRef, HostBinding } from '@angular/core';
import { TdMediaService } from '@covalent/core';
import { slideInDownAnimation } from './../../animations';
import { CredentialsDto } from './../../services/api.services';
import { AuthService } from './../../services/auth.service';
import { Router, NavigationExtras } from '@angular/router';
import { ErrorInfo } from './../../common/errorInfo';

@Component({
	selector: 'login',
	templateUrl: './login.component.html',
	animations: [slideInDownAnimation],
})
export class LoginComponent implements OnInit {
	@HostBinding('@routeAnimation') routeAnimation = true;
	public credentials: CredentialsDto = <CredentialsDto>{};
	private error: ErrorInfo;

	constructor(
		public media: TdMediaService,
		private _authService: AuthService,
		private _changeDetectionRef: ChangeDetectorRef,
		private router: Router
	) {
	}

	submit() {
		this._authService.login(this.credentials)
			.subscribe(result => {
				// Get the redirect URL from our auth service
				// If no redirect has been set, use the default-
				let redirect = this._authService.redirectUrl ? this._authService.redirectUrl : '/samples';

				// Set our navigation extras object
				// that passes on our global query params and fragment
				let navigationExtras: NavigationExtras = {
					queryParamsHandling: 'preserve',
					preserveFragment: true
				};

				// Redirect the user
				this.router.navigate([redirect], navigationExtras);
			}, error => {
				this.error = error;
				console.log('Error - AuthenticationService.login: ' + error);
			});
	}

	ngOnInit() {
	}

	ngAfterViewInit(): void {
		this.media.broadcast();
		this._changeDetectionRef.detectChanges();
	}
}
