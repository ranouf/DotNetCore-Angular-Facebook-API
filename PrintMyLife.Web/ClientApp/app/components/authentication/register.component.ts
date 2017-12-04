import { Component, ViewEncapsulation, OnInit, ChangeDetectorRef, HostBinding } from '@angular/core';
import { slideInDownAnimation }   from './../../animations';
import { RegistrationDto, AccountService } from './../../services/api.services';
import { Router } from '@angular/router';
import { ErrorInfo } from './../../common/errorInfo';

@Component({
	selector: 'register',
	templateUrl: './register.component.html',
  providers: [AccountService],
  animations: [ slideInDownAnimation ]
})
export class RegisterComponent implements OnInit {
  @HostBinding('@routeAnimation') routeAnimation = true;
  public registration: RegistrationDto = <RegistrationDto>{};
  private error: ErrorInfo;

	constructor(
		private accountService: AccountService,
		private router: Router, 
		private _changeDetectionRef: ChangeDetectorRef,
  ) {
	}

	submit() {
		this.accountService.register(this.registration)
			.subscribe(result => {
				this.router.navigate(['/login']);
			}, error => {
				this.error = error;
				console.log('Error - AuthenticationService.register: ' + error);
			});
	}

	ngOnInit() {
	}

	ngAfterViewInit(): void {
		this._changeDetectionRef.detectChanges();
	}
}
