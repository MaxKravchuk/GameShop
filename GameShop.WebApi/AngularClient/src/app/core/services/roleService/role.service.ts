import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { UtilsService } from "../helpers/utilsService/utils-service";
import { catchError, Observable } from "rxjs";
import { Role } from "../../models/Role";
import { PagedList } from "../../models/PagedList";

@Injectable({
  providedIn: 'root'
})
export class RoleService {

    private apiUrl: string = "/api/roles/";

    constructor(
        private http: HttpClient,
        private utilsService: UtilsService
    ) { }

    getAllRolesPaged(pagedParams: any): Observable<PagedList<Role>> {
        return this.http.get<PagedList<Role>>(`${this.apiUrl}getAllPaged`, {params: pagedParams})
            .pipe(
                catchError(err => {
                    this.utilsService.handleError(err);
                    return [];
                })
            );
    }

    getAllRoles(): Observable<Role[]> {
        return this.http.get<Role[]>(`${this.apiUrl}getAll`)
            .pipe(
                catchError(err => {
                    this.utilsService.handleError(err);
                    return [];
                })
            );
    }

    createRole(role: Role): Observable<Role> {
        return this.http.post<Role>(`${this.apiUrl}`, role)
            .pipe(
                catchError(err => {
                    this.utilsService.handleError(err);
                    return [];
                })
            );
    }

    updateRole(role: Role): Observable<Role> {
        return this.http.put<Role>(`${this.apiUrl}`, role)
            .pipe(
                catchError(err => {
                    this.utilsService.handleError(err);
                    return [];
                })
            );
    }

    deleteRole(id: number): Observable<Role> {
        return this.http.delete<Role>(`${this.apiUrl}`, {params: {roleId: id}})
            .pipe(
                catchError(err => {
                    this.utilsService.handleError(err);
                    return [];
                })
            );
    }
}
