import { Injectable } from '@angular/core';
import {ResourseService} from "../../../core/services/ResourseService/resourse.service";
import {Location} from "@angular/common";
import {HttpClient} from "@angular/common/http";
import {Comment} from "../../../core/models/Comment";
import {catchError, Observable} from "rxjs";
import {map} from "rxjs/operators";

@Injectable({
  providedIn: 'root'
})
export class CommentService extends ResourseService<Comment>{

  constructor(
    private httpClient: HttpClient,
    private currentLocation: Location){
    super(httpClient, currentLocation, Comment, '/api/comments/');
  }

  getCommentsByGameKey(gameKey: string): Observable<Comment[]>{
    return this.http.get<Comment[]>(`${this.apiUrl}getAllByGameKey/${gameKey}`,this.httpOptions)
      .pipe(
        map((result) => result.map((item) => new this.tConstructor(item))),
        catchError(this.handleError<Comment[]>('getAllByGameKey'))
      );
  }
}
