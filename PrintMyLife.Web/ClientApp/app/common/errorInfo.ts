import { Response } from "@angular/http";

export class ErrorInfo {
	message: string;
	response: Response;

	constructor() {
		this.reset();
	}

	reset() {
		this.message = "";
	}

	parseResponseError(response): ErrorInfo {
		if (response.hasOwnProperty("message"))
			return response;
		if (response.hasOwnProperty("Message")) {
			response.message = response.Message;
			return response;
		}

		let err = new ErrorInfo();
		err.response = response;
		err.message = response.statusText;

		try {
			let data = response.json();
			if (data && data.message)
				err.message = data.message;
		}
		catch (ex) { }

		if (!err.message)
			err.message = "Unknown server failure.";

		return err;
	}
}
