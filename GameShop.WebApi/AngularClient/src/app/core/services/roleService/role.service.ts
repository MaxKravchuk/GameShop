import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { UtilsService } from "../helpers/utilsService/utils-service";
import { catchError, Observable } from "rxjs";
import { Role } from "../../models/Role";

@Injectable({
  providedIn: 'root'
})
export class RoleService {

    private apiUrl: string = "/api/roles/";

    constructor(
        private http: HttpClient,
        private utilsService: UtilsService
    ) { }

    getAllRoles(): Observable<Role[]> {
        return this.http.get<Role[]>(`${this.apiUrl}getAll`)
            .pipe(
                catchError(err => {
                    this.utilsService.handleError(err);
                    return [];
                })
            );
    }
}
