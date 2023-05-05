import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { Comment } from "../../models/Comment";
import { catchError, Observable } from "rxjs";
import { UtilsService } from "../helpers/utilsService/utils-service";

@Injectable({
    providedIn: 'root'
})
export class CommentService {

    private apiUrl: string = "/api/comments/";

    constructor(
        private http: HttpClient,
        private utilsService: UtilsService
    ) {}

    getCommentsByGameKey(gameKey: string): Observable<Comment[]> {
        return this.http.get<Comment[]>(`${this.apiUrl}getAllByGameKey/${gameKey}`)
            .pipe(
                catchError(err => {
                    this.utilsService.openWithMessage(err.message);
                    return [];
                })
            );
    }

    createComment(comment: Comment): Observable<Comment> {
        return this.http.post<Comment>(`${this.apiUrl}leaveComment`, comment)
            .pipe(
                catchError(err => {
                    this.utilsService.openWithMessage(err.message);
                    return [];
                })
            );
    }
}
