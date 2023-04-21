import { Injectable } from '@angular/core';
import {ResourseService} from "../ResourseService/resourse.service";
import {PlatformType} from "../../models/PlatformType";
import {HttpClient} from "@angular/common/http";
import {Location} from "@angular/common";
import {map} from "rxjs/operators";
import {catchError, Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class PlatformTypeService extends ResourseService<PlatformType>{
  constructor(
    private httpClient: HttpClient,
    private currentLocation: Location){
    super(httpClient, currentLocation, PlatformType, '/api/platformTypes/');
  }

  getAllPlatformTypes():Observable<PlatformType[]>{
    return this.http.get<PlatformType[]>(`${this.apiUrl}`,this.httpOptions)
      .pipe(
        map((result) => result.map((item) => new this.tConstructor(item))),
        catchError(this.handleError<PlatformType[]>('getAll'))
      );
  }
}
