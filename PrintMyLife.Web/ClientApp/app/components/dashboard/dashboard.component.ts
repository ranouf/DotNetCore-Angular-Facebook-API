import { Component, ViewEncapsulation, OnInit, ChangeDetectorRef, HostBinding } from '@angular/core';
import { TdMediaService } from '@covalent/core';
import { slideInDownAnimation }   from './../../animations';

@Component({
	selector: 'dashboard',
	templateUrl: './dashboard.component.html',
  animations: [ slideInDownAnimation ]
})
export class DashboardComponent implements OnInit {
  @HostBinding('@routeAnimation') routeAnimation = true;

	constructor(
    public media: TdMediaService,
		private _changeDetectionRef: ChangeDetectorRef) {
	}

	ngOnInit() {
	}

	ngAfterViewInit(): void {
		this.media.broadcast();
		this._changeDetectionRef.detectChanges();
	}
}
