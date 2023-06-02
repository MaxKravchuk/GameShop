import { Injectable } from '@angular/core';
import { Game } from "../../models/Game";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { catchError, Observable } from "rxjs";
import { CreateGameModel } from "../../models/CreateGameModel";
import { UtilsService } from "../helpers/utilsService/utils-service";
import { PagedList } from "../../models/PagedList";


@Injectable({
    providedIn: 'root'
})
export class GameService {

    private apiUrl: string = "/api/games/";

    constructor(
        private http: HttpClient,
        private utilsService: UtilsService
    ) {}

    getAllGames(filterParams: any): Observable<PagedList<Game>> {
        return this.http.get<PagedList<Game>>(`${this.apiUrl}getAll/`, {params: filterParams})
            .pipe(
                catchError(err => {
                    this.utilsService.handleError(err);
                    return [];
                })
            );
    }

    getGameDetailsByKey(key: string): Observable<Game> {
        return this.http.get<Game>(`${this.apiUrl}getDetailsByKey/${key}`)
            .pipe(
                catchError(err => {
                    this.utilsService.handleError(err);
                    return [];
                })
            );
    }

    downloadGame(gameKey: string): Observable<any> {
        const headers: HttpHeaders = new HttpHeaders({
            'Content-Type': 'application/json',
            'Accept': 'application/octet-stream'
        });

        return this.http.get(`${this.apiUrl}downloadGame/${gameKey}`, {
            headers,
            responseType: 'blob'
        }).pipe(
            catchError(err => {
                this.utilsService.handleError(err);
                return [];
            })
        );
    }

    createGame(createGameDTO: CreateGameModel): Observable<CreateGameModel> {
        return this.http.post<CreateGameModel>(`${this.apiUrl}`, createGameDTO)
            .pipe(
                catchError(err => {
                    this.utilsService.handleError(err);
                    return [];
                })
            );
    }

    getNumberOfGames(): Observable<number> {
        return this.http.get<number>(`${this.apiUrl}numberOfGames`)
            .pipe(
                catchError(err => {
                    this.utilsService.handleError(err);
                    return [];
                })
            );
    }

    updateGame(game: Game): Observable<Game> {
        return this.http.put<Game>(`${this.apiUrl}`, game)
            .pipe(
                catchError(err => {
                    this.utilsService.handleError(err);
                    return [];
                })
            );
    }

    deleteGame(gameKey: string): Observable<Game> {
        return this.http.delete<Game>(`${this.apiUrl}delete/${gameKey}`)
            .pipe(
                catchError(err => {
                    this.utilsService.handleError(err);
                    return [];
                })
            );
    }
}
