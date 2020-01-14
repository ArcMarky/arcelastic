import { Injectable } from '@angular/core';
import { Http, RequestOptionsArgs, Response } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import 'rxjs/add/operator/map';
import * as _ from 'lodash';
import { NgxSpinnerService } from 'ngx-spinner';
@Injectable()
export class HttpService {

  constructor(private _http: Http, private _spinner: NgxSpinnerService) { }


  get(url: string, options?: RequestOptionsArgs): Observable<any> {
    this._spinner.show();
    return this._http.get(url, options).finally(() => { this._spinner.hide(); });
  }

  post(url: string, body: string, options?: RequestOptionsArgs): Observable<any> {
    this._spinner.show();
    return this._http.post(url, body, options).finally(() => { this._spinner.hide(); });
  }

  delete(url: string, options?: RequestOptionsArgs): Observable<Response> {
    this._spinner.show();
    return this._http.delete(url, options).finally(() => { this._spinner.hide();});
  }

  handleError(error: Response | any) {
    let errorMessage: any;
    console.log(error);
    if (error instanceof Response) {
      if (error.status !== 0) {
        try {
          errorMessage = [{ field: 'custom', message: (<any>error)._body || error.statusText || error.text }];
        } catch (exception) {
          errorMessage = [{ field: 'custom', message: 'Oops! Something went wrong, please try again!' }];
        }
      } else {
        errorMessage = [{ field: 'custom', message: 'Oops! Something went wrong, please try again!' }];
      }
    } else {
      errorMessage = [{ field: 'custom', message: 'Oops! Something went wrong, please try again!' }];
    }
    return Observable.throw(errorMessage).toPromise();
  }
}
