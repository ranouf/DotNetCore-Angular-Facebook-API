import { Component, OnInit } from '@angular/core';
import { TdMediaService } from '@covalent/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
	constructor(
    public media: TdMediaService) {
	}

	ngOnInit() {
	}

	ngAfterViewInit(): void {
		this.media.broadcast();
	}
}
