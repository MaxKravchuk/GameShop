import { Injectable } from '@angular/core';
import { Game } from "../../models/Game";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { catchError, Observable } from "rxjs";
import { CreateGameModel } from "../../models/CreateGameModel";
import { UtilsService } from "../helpers/utilsService/utils-service";


@Injectable({
    providedIn: 'root'
})
export class GameService {

    private apiUrl: string = "/api/games/";

    constructor(
        private http: HttpClient,
        private utilsService: UtilsService
    ) {}

    getAllGames(): Observable<Game[]> {
        return this.http.get<Game[]>(`${this.apiUrl}getAll`)
            .pipe(
                catchError(err => {
                    this.utilsService.openWithMessage(err.message);
                    return [];
                })
            );
    }

    getGameDetailsByKey(key: string): Observable<Game> {
        return this.http.get<Game>(`${this.apiUrl}getDetailsByKey/${key}`)
            .pipe(
                catchError(err => {
                    this.utilsService.openWithMessage(err.message);
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
                this.utilsService.openWithMessage(err.message);
                return [];
            })
        );
    }

    createGame(createGameDTO: CreateGameModel): Observable<CreateGameModel> {
        return this.http.post<CreateGameModel>(`${this.apiUrl}`, createGameDTO)
            .pipe(
                catchError(err => {
                    this.utilsService.openWithMessage(err.message);
                    return [];
                })
            );
    }

    getNumberOfGames(): Observable<number> {
        return this.http.get<number>(`${this.apiUrl}numberOfGames`)
            .pipe(
                catchError(err => {
                    this.utilsService.openWithMessage(err.message);
                    return [];
                })
            );
    }
}
