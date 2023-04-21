import { Injectable } from '@angular/core';
import {ResourseService} from "../ResourseService/resourse.service";
import {Genre} from "../../models/Genre";
import {HttpClient} from "@angular/common/http";
import {Location} from "@angular/common";
import {catchError, Observable} from "rxjs";
import {map} from "rxjs/operators";

@Injectable({
  providedIn: 'root'
})
export class GenreService extends ResourseService<Genre>{

  constructor(
    private httpClient: HttpClient,
    private currentLocation: Location){
    super(httpClient, currentLocation, Genre, '/api/genres/');
  }

  getAllGenres():Observable<Genre[]>{
    return this.http.get<Genre[]>(`${this.apiUrl}`,this.httpOptions)
      .pipe(
        map((result) => result.map((item) => new this.tConstructor(item))),
        catchError(this.handleError<Genre[]>('getAll'))
      );
  }
}
