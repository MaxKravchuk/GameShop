import { Injectable } from '@angular/core';
import { Observable, Subject } from "rxjs";
import { CommentShared } from "../../../models/helpers/CommentShared";

@Injectable({
    providedIn: 'root'
})
export class SharedCommentService {

    private dataSubject: Subject<CommentShared> = new Subject<CommentShared>();

    private reloadCommentsSource: Subject<void> = new Subject<void>();

    reloadComments$: Observable<any> = this.reloadCommentsSource.asObservable();

    sendData(model: CommentShared) {
        this.dataSubject.next(model);
    }

    getData() {
        return this.dataSubject.asObservable();
    }

    reloadComments() {
        this.reloadCommentsSource.next();
    }
}
