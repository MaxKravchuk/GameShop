import { Injectable } from '@angular/core';
import {Subject} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class SharedCommentService {

  private dataSubject = new Subject<string>();
  private reloadCommentsSource = new Subject<void>();
  reloadComments$ = this.reloadCommentsSource.asObservable();
  sendData(data: string) {
    this.dataSubject.next(data);
  }
  getData() {
    return this.dataSubject.asObservable();
  }

  reloadComments() {
    this.reloadCommentsSource.next();
  }
}
