import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { UtilsService } from "../helpers/utilsService/utils-service";
import { catchError, Observable } from "rxjs";
import { Genre } from "../../models/Genre";
import { User } from "../../models/User";
import { PagedList } from "../../models/PagedList";

@Injectable({
  providedIn: 'root'
})
export class UserService {

    private apiUrl: string = "/api/users/";

    constructor(
        private http: HttpClient,
        private utilsService: UtilsService
    ) { }

    getAllUsersPaged(pagedParams: any): Observable<PagedList<User>> {
        return this.http.get<PagedList<User>>(`${this.apiUrl}getAllPaged`, {params: pagedParams})
            .pipe(
                catchError(err => {
                    this.utilsService.handleError(err);
                    return [];
                })
            );
    }

    getAllUsers(): Observable<User[]> {
        return this.http.get<User[]>(`${this.apiUrl}getAll`)
            .pipe(
                catchError(err => {
                    this.utilsService.handleError(err);
                    return [];
                })
            );
    }

    createUserWithRole(user: User): Observable<User>{
        return this.http.post<User>(`${this.apiUrl}createUserWithRole`, user)
            .pipe(
                catchError(err => {
                    this.utilsService.handleError(err);
                    return [];
                })
            );
    }

    updateUser(user: User): Observable<User> {
        return this.http.put<User>(`${this.apiUrl}`, user)
            .pipe(
                catchError(err => {
                    this.utilsService.handleError(err);
                    return [];
                })
            );
    }

    deleteUser(id: number): Observable<User> {
        return this.http.delete<User>(`${this.apiUrl}/deleteUser/${id}`)
            .pipe(
                catchError(err => {
                    this.utilsService.handleError(err);
                    return [];
                })
            );
    }

    IsAnExistingUserBannedAsync(nickName: string): Observable<boolean> {
        return this.http.get<boolean>(`${this.apiUrl}isBanned/${nickName}`)
            .pipe(
                catchError(err => {
                    this.utilsService.handleError(err);
                    return [];
                })
            );
    }

    banUser(user: User): Observable<User> {
        return this.http.put<User>(`${this.apiUrl}banUser`, user)
            .pipe(
                catchError(err => {
                    this.utilsService.handleError(err);
                    return [];
                })
            );
    }
}
