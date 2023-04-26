import { Injectable } from '@angular/core';
import {ResourseService} from "../ResourseService/resourse.service";
import {Publisher} from "../../models/Publisher";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Location} from "@angular/common";
import {catchError, Observable} from "rxjs";
import {map} from "rxjs/operators";

@Injectable({
  providedIn: 'root'
})
export class PublisherService extends ResourseService<Publisher>{
  constructor(
    private httpClient: HttpClient,
    private currentLocation: Location){
    super(httpClient, currentLocation, Publisher, '/api/publishers/');
  }

  getPublisherByCompanyName(companyName:string):Observable<Publisher>{
    return this.http.get<Publisher>(`${this.apiUrl}?companyName=${companyName}`, this.httpOptions)
      .pipe(
        map((result) => new this.tConstructor(result)),
        catchError(this.handleError<Publisher>("getOne"))
      );
  }

  createPublisher(publisher: Publisher):Observable<Publisher>{
    return this.http.post<Publisher>(`${this.apiUrl}`, publisher, this.httpOptions)
      .pipe(
        catchError(this.handleError<Publisher>("createPublisher"))
      );
  }

  getAllPublishers():Observable<Publisher[]>{
    return this.http.get<Publisher[]>(`${this.apiUrl}getAll`, this.httpOptions)
      .pipe(
        map((result) => result.map((item) => new this.tConstructor(item))),
        catchError(this.handleError<Publisher[]>("getAll"))
      );
  }
}
