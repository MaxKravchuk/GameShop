import { Injectable } from '@angular/core';
import {Game} from "../../../core/models/Game";
import {HttpClient} from "@angular/common/http";
import {ResourseService} from "../../../core/services/ResourseService/resourse.service";
import {Location} from "@angular/common";
import {Observable} from "rxjs";
@Injectable({
  providedIn: 'root'
})
export class MainPageService extends  ResourseService<Game>{
  constructor(
    private httpClient: HttpClient,
    private currentLocation: Location){
    super(httpClient, currentLocation, Game, '/api/games/getAll');
  }

  getGames(): Observable<Game[]>{
    return this.getAll();
  }
}
