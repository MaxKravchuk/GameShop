import { Injectable } from '@angular/core';
import {ResourseService} from "../ResourseService/resourse.service";
import {Game} from "../../models/Game";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Location} from "@angular/common";
import {catchError, Observable} from "rxjs";
import {map} from "rxjs/operators";
import {CreateGameDTO} from "../../DTOs/GameDTOs/CreateGameDTO";


@Injectable({
  providedIn: 'root'
})
export class GameService extends ResourseService<Game>{
  constructor(
    private httpClient: HttpClient,
    private currentLocation: Location){
    super(httpClient, currentLocation, Game, '/api/games/');
  }

  getGameDetailsByKey(key: string): Observable<Game>{
    return this.http.get<Game>(`${this.apiUrl}getDetailsByKey/${key}`,this.httpOptions)
      .pipe(
        map((result) => new this.tConstructor(result)),
        catchError(this.handleError<Game>('getOne'))
      );
  }

  downloadGame(gameKey: string): Observable<any> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/octet-stream'
    });

    return this.http.get(`${this.apiUrl}downloadGame/${gameKey}`, {
      headers,
      responseType: 'blob'
    }).pipe(
      catchError(this.handleError<any>('downloadGame'))
    );
  }

  createGame(createGameDTO: CreateGameDTO): Observable<CreateGameDTO> {
    return this.http.post<CreateGameDTO>(`${this.apiUrl}`, createGameDTO, this.httpOptions)
      .pipe(
        catchError(this.handleError<CreateGameDTO>('createGame'))
      );
  }
}
