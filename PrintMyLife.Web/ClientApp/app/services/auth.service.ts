import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService, CredentialsDto, UserAuthenticationDto } from './api.services';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/fromPromise';

@Injectable()
export class AuthService {
	// store the URL so we can redirect after logging in
	redirectUrl: string;

	constructor(
		private router: Router, 
		private authenticationService: AuthenticationService,
	) {
	}

	public login(credentials: CredentialsDto): Observable<UserAuthenticationDto | null> {
		return Observable.create(observer => {
			this.authenticationService.login(credentials)
				.subscribe(result => {
					localStorage.setItem('id_token', result.token);
					localStorage.setItem('firstname', result.firstname);
					localStorage.setItem('lastname', result.lastname);
					localStorage.setItem('expiration_date', result.expirationDate.toString());
					observer.next(result);
				}, error => {
					console.log('Error - AuthenticationService.login: ' + error);
					observer.error(error);
				});
		})
	}

	public isLoggedIn(): boolean {
		var item = localStorage.getItem('expiration_date');
		if (item) {
			var expirationDate = new Date(item);
			if (expirationDate && expirationDate.getTime() > new Date().getTime() ) {
				return true;
			}
      //token expired
			this.logout();
		}

		// no token or token expired
		return false;
	}

	public logout(): void {
		localStorage.clear();
		this.router.navigate(['/login']);
	}
}
