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
        private utilsService: UtilsService
    ) {}

    getAllGenres(): Observable<Genre[]> {
        return this.http.get<Genre[]>(`${this.apiUrl}getAll`)
            .pipe(
                catchError(err => {
                    this.utilsService.handleError(err);
                    return [];
                })
            );
    }

    createGenre(genre: Genre): Observable<Genre> {
        return this.http.post<Genre>(`${this.apiUrl}`, genre)
            .pipe(
                catchError(err => {
                    this.utilsService.handleError(err);
                    return [];
                })
            );
    }

    updateGenre(genre: Genre): Observable<Genre> {
        return this.http.put<Genre>(`${this.apiUrl}`, genre)
            .pipe(
                catchError(err => {
                    this.utilsService.handleError(err);
                    return [];
                })
            );
    }

    deleteGenre(id: number): Observable<Genre> {
        return this.http.delete<Genre>(`${this.apiUrl}${id}`)
            .pipe(
                catchError(err => {
                    this.utilsService.handleError(err);
                    return [];
                })
            );
    }
}
