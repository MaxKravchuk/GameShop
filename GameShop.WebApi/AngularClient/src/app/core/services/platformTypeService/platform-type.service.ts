import { Injectable } from '@angular/core';
import { PlatformType } from "../../models/PlatformType";
import { HttpClient } from "@angular/common/http";
import { catchError, Observable } from "rxjs";
import { UtilsService } from "../helpers/utilsService/utils-service";

@Injectable({
    providedIn: 'root'
})
export class PlatformTypeService {

    private apiUrl: string = "/api/platformTypes/";

    constructor(
        private http: HttpClient,
        private utilsService: UtilsService
    ) {}

    getAllPlatformTypes(): Observable<PlatformType[]> {
        return this.http.get<PlatformType[]>(`${this.apiUrl}getAll`)
            .pipe(
                catchError(err => {
                    this.utilsService.handleError(err);
                    return [];
                })
            );
    }

    createPlatformType(platformType: PlatformType): Observable<PlatformType> {
        return this.http.post<PlatformType>(`${this.apiUrl}`, platformType)
            .pipe(
                catchError(err => {
                    this.utilsService.handleError(err);
                    return [];
                })
            );
    }

    updatePlatformType(platformType: PlatformType): Observable<PlatformType> {
        return this.http.put<PlatformType>(`${this.apiUrl}`, platformType)
            .pipe(
                catchError(err => {
                    this.utilsService.handleError(err);
                    return [];
                })
            );
    }

    deletePlatformType(id: number): Observable<PlatformType> {
        return this.http.delete<PlatformType>(`${this.apiUrl}${id}`)
            .pipe(
                catchError(err => {
                    this.utilsService.handleError(err);
                    return [];
                })
            );
    }
}
