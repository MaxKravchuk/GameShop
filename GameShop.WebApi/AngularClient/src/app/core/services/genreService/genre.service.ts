import { Injectable } from '@angular/core';
import { Genre } from "../../models/Genre";
import { HttpClient } from "@angular/common/http";
import { catchError, Observable } from "rxjs";
import { UtilsService } from "../helpers/utilsService/utils-service";

@Injectable({
    providedIn: 'root'
})
export class GenreService {

    private apiUrl: string = "/api/genres/";

    constructor(
        private http: HttpClient,
        private utilsService: UtilsService) {
    }

    public getAllGenres(): Observable<Genre[]> {
        return this.http.get<Genre[]>(`${this.apiUrl}`)
            .pipe(
                catchError(err => {
                    this.utilsService.openWithMessage(err.message);
                    return [];
                })
            );
    }
}
