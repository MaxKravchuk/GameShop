import { Injectable } from '@angular/core';
import { Publisher } from "../../models/Publisher";
import { HttpClient } from "@angular/common/http";
import { catchError, Observable } from "rxjs";
import { UtilsService } from "../helpers/utilsService/utils-service";

@Injectable({
    providedIn: 'root'
})
export class PublisherService {

    private apiUrl: string = "/api/publishers/";

    constructor(
        private http: HttpClient,
        private utilsService: UtilsService) {
    }

    public getPublisherByCompanyName(companyName: string): Observable<Publisher> {
        return this.http.get<Publisher>(`${this.apiUrl}?companyName=${companyName}`)
            .pipe(
                catchError(err => {
                    this.utilsService.openWithMessage(err.message);
                    return [];
                })
            );
    }

    public createPublisher(publisher: Publisher): Observable<Publisher> {
        return this.http.post<Publisher>(`${this.apiUrl}`, publisher)
            .pipe(
                catchError(err => {
                    this.utilsService.openWithMessage(err.message);
                    return [];
                })
            );
    }

    public getAllPublishers(): Observable<Publisher[]> {
        return this.http.get<Publisher[]>(`${this.apiUrl}getAll`)
            .pipe(
                catchError(err => {
                    this.utilsService.openWithMessage(err.message);
                    return [];
                })
            );
    }
}
