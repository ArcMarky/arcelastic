"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var http_1 = require("@angular/http");
var Rx_1 = require("rxjs/Rx");
require("rxjs/add/operator/map");
var HttpService = /** @class */ (function () {
    function HttpService(_http, _spinner) {
        this._http = _http;
        this._spinner = _spinner;
    }
    HttpService.prototype.get = function (url, options) {
        var _this = this;
        this._spinner.show();
        return this._http.get(url, options).finally(function () { _this._spinner.hide(); });
    };
    HttpService.prototype.post = function (url, body, options) {
        var _this = this;
        this._spinner.show();
        return this._http.post(url, body, options).finally(function () { _this._spinner.hide(); });
    };
    HttpService.prototype.delete = function (url, options) {
        var _this = this;
        this._spinner.show();
        return this._http.delete(url, options).finally(function () { _this._spinner.hide(); });
    };
    HttpService.prototype.handleError = function (error) {
        var errorMessage;
        console.log(error);
        if (error instanceof http_1.Response) {
            if (error.status !== 0) {
                try {
                    errorMessage = [{ field: 'custom', message: error._body || error.statusText || error.text }];
                }
                catch (exception) {
                    errorMessage = [{ field: 'custom', message: 'Oops! Something went wrong, please try again!' }];
                }
            }
            else {
                errorMessage = [{ field: 'custom', message: 'Oops! Something went wrong, please try again!' }];
            }
        }
        else {
            errorMessage = [{ field: 'custom', message: 'Oops! Something went wrong, please try again!' }];
        }
        return Rx_1.Observable.throw(errorMessage).toPromise();
    };
    HttpService = __decorate([
        core_1.Injectable()
    ], HttpService);
    return HttpService;
}());
exports.HttpService = HttpService;
//# sourceMappingURL=http.service.js.map