import { Injectable } from '@angular/core';
import { Observable, Subject } from "rxjs";

@Injectable({
    providedIn: 'root'
})
export class SharedService<T> {

    private commentSharedSubject: Subject<T> = new Subject<T>();

    private reloadSourceSubject: Subject<void> = new Subject<void>();

    reloadSource$: Observable<any> = this.reloadSourceSubject.asObservable();

    sendData(model: T): void {
        this.commentSharedSubject.next(model);
    }

    getData$(): Observable<T> {
        return this.commentSharedSubject.asObservable();
    }

    reloadSource(): void {
        this.reloadSourceSubject.next();
    }
}
