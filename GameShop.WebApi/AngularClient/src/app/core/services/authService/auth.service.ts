import { Injectable } from '@angular/core';
import { UtilsService } from "../helpers/utilsService/utils-service";
import { HttpClient } from "@angular/common/http";
import { BehaviorSubject, catchError, Observable, tap } from "rxjs";
import { AuthenticatedResponseModel } from "../../models/AuthModels/AuthenticatedResponseModel";
import { TokenApiModel } from "../../models/AuthModels/TokenApiModel";
import jwtDecode from 'jwt-decode';
import { RegistrationModel } from "../../models/AuthModels/RegistrationModel";

@Injectable({
  providedIn: 'root'
})
export class AuthService {

    private authUrl: string = '/api/auth/';
    private tokenUrl: string = '/api/tokens/';
    private userRoleSub: BehaviorSubject<string | null> = new BehaviorSubject<string | null>(this.getRole());
    private isAuthorizedSub: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(this.isAuthorized());

    constructor(
        private http: HttpClient,
        private utilsService: UtilsService
    ) { }

    login(loginParams: any) : Observable<AuthenticatedResponseModel> {
        return this.http.get<AuthenticatedResponseModel>(`${this.authUrl}login/`, {params: loginParams})
            .pipe(
                tap(
                    (res: AuthenticatedResponseModel): void => {
                        this.setSession(res);
                        this.userRoleSub.next(this.getRole());
                        this.isAuthorizedSub.next(this.isAuthorized());
                    }
                ),
                catchError(err => {
                    this.utilsService.handleError(err);
                    return [];
                }
            )
        );
    }

    register(registrationModel: RegistrationModel) : Observable<boolean> {
        return this.http.post<boolean>(`${this.authUrl}register`, registrationModel)
            .pipe(
                catchError(err => {
                    this.utilsService.handleError(err);
                    return [];
                }
            )
        );
    }

    refreshToken(tokenApiModel: TokenApiModel) : Observable<AuthenticatedResponseModel> {
        return this.http.post<AuthenticatedResponseModel>(`${this.tokenUrl}refresh`, tokenApiModel)
            .pipe(
                catchError(err => {
                    this.utilsService.handleError(err);
                    return [];
                }
            )
        );
    }

    logout(): void {
        localStorage.removeItem('access_token');
        localStorage.removeItem('refresh_token');
        this.isAuthorizedSub.next(false);
        this.userRoleSub.next(this.getRole());
    }

    getUserId() {
        const tokenStr = localStorage.getItem('access_token');
        if (tokenStr != null){
            try{
                const bearerToken : any = jwtDecode(tokenStr);
                return bearerToken.Id;
            }
            catch(err){
                return null;
            }
        }
    }

    getRole() {
        const tokenStr = localStorage.getItem('access_token');
        if (tokenStr != null){
            try{
                const bearerToken : any = jwtDecode(tokenStr);
                return bearerToken.Role;
            }
            catch(err){
                return null;
            }
        }
    }

    getUserName() {
        const tokenStr = localStorage.getItem('access_token');
        if (tokenStr != null){
            try{
                const bearerToken : any = jwtDecode(tokenStr);
                return bearerToken.UserName;
            }
            catch(err){
                return null;
            }
        }
    }

    isInRole(role:string): boolean {
        return role === this.getRole()?.trim();
    }

    getUserRole$(): Observable<string | null> {
        return this.userRoleSub.asObservable();
    }

    getAccessToken(): string | null {
        return localStorage.getItem('access_token');
    }

    setAccessToken(accessToken:string): void {
        localStorage.setItem('access_token', accessToken);
    }

    setRefreshToken(refreshToken:string): void {
        localStorage.setItem('refresh_token', refreshToken);
    }

    getRefreshToken(): string | null {
        return localStorage.getItem('refresh_token');
    }

    isAuthorized(): boolean {
        return localStorage.getItem('access_token') != null
    }

    getIsAuthorized$(): Observable<boolean> {
        return this.isAuthorizedSub.asObservable();
    }

    private setSession(authResult: AuthenticatedResponseModel): void {
        const tokenResponse: AuthenticatedResponseModel = authResult as AuthenticatedResponseModel;

        localStorage.setItem('access_token', tokenResponse.Token);
        if (tokenResponse.RefreshToken != null)
            localStorage.setItem('refresh_token', tokenResponse.RefreshToken);
    }
}
