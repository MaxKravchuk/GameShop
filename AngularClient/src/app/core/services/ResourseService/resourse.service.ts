import {ResourceModel} from "../../models/resource-model";
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { catchError, Observable, of } from 'rxjs';
import {map} from "rxjs/operators";
import {Location} from "@angular/common";

export abstract class ResourseService<T extends ResourceModel<T>> {

  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
    }),
  };
  protected constructor(
    protected http: HttpClient,
    protected location: Location,
    protected tConstructor: { new(model: Partial<T>): T },
    protected apiUrl: string) {}

  getAll(): Observable<T[]> {
    return this.http.get<T[]>(this.apiUrl)
      .pipe(
        map((result) => result.map((i) => new this.tConstructor(i))),
        catchError(this.handleError<T[]>('getAll', []))
      );
  }

  getOne(UID: any): Observable<T> {
    return this.http.get<T>(`${this.apiUrl}/${UID}`)
      .pipe(
        map((result) => new this.tConstructor(result)),
        catchError(this.handleError<T>('getOne'))
      );
  }


  protected handleError<T>(operation: string = 'operation', result?: T) {
    return (error: HttpErrorResponse): Observable<T> => {
      switch (error.status) {
        case 0:
          console.warn(`During ${operation} a client-side error occurred: `, error.error);
          break;
        case 401:
          console.warn(`During ${operation}: Unauthorized access, code ${error.status}, body was: `, error.error);
          break;
        case 403:
          console.warn(`During ${operation}: Forbidden, code ${error.status}, body was: `, error.error);
          break;
        case 404:
          console.warn(`During ${operation}: Resource is not available code ${error.status}, body was: `, error.error);
          break;
        case 500:
          console.error(`During ${operation}: something went wrong on the server! Body was: `, error.error);
          break;
      }

      return of(result as T);
    }
  }

  goToPrevPage():void{
    this.location.back();
  }
}
