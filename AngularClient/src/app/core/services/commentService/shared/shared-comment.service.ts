import { Injectable } from '@angular/core';
import {Subject} from "rxjs";
import {CommentShared} from "../../../models/Helpers/CommentShared";

@Injectable({
  providedIn: 'root'
})
export class SharedCommentService {

  private dataSubject = new Subject<CommentShared>();
  private reloadCommentsSource = new Subject<void>();
  reloadComments$ = this.reloadCommentsSource.asObservable();
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
