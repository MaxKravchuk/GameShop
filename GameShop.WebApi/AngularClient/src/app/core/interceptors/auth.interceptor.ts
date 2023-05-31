import { Injectable } from '@angular/core';
import {
    HttpRequest,
    HttpHandler,
    HttpEvent,
    HttpInterceptor, HttpErrorResponse
} from '@angular/common/http';
import { BehaviorSubject, catchError, filter, Observable, switchMap, take, throwError } from 'rxjs';
import { AuthService } from "../services/authService/auth.service";
import { TokenApiModel } from "../models/AuthModels/TokenApiModel";
import { AuthenticatedResponseModel } from "../models/AuthModels/AuthenticatedResponseModel";
import { Router } from "@angular/router";

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

    private isRefreshing: boolean = false;
    private refreshTokenSubject: BehaviorSubject<any> = new BehaviorSubject<any>(null);

    constructor(
        private authService: AuthService,
        private router: Router) {}

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const accessToken = this.authService.getAccessToken();
        const refreshToken = this.authService.getRefreshToken();

        if (accessToken) {
            request = this.addAuthorizationHeader(request, accessToken);
        }

        return next.handle(request).pipe(
            catchError((error) => {
                if (error.status === 401 && refreshToken) {
                    const data: TokenApiModel = {
                        AccessToken: accessToken,
                        RefreshToken: refreshToken
                    };
                    return this.authService.refreshToken(data).pipe(
                        switchMap((response: AuthenticatedResponseModel) => {
                            const newAccessToken: string = response.Token;
                            this.authService.setAccessToken(response.Token);
                            this.authService.setRefreshToken(response.RefreshToken);
                            request = this.addAuthorizationHeader(request, newAccessToken);
                            return next.handle(request);
                        }),
                        catchError((refreshError) => {
                            this.authService.logout();
                            this.router.navigate(['/auth/login']);
                            return throwError(refreshError);
                        })
                    );
                } else {
                    return throwError(error);
                }
            })
        );
    }

    private addAuthorizationHeader(request: HttpRequest<any>, accessToken: string): HttpRequest<any> {
        return request.clone({
            headers: request.headers.set("Authorization", "Bearer " + accessToken)
        });
    }
}
