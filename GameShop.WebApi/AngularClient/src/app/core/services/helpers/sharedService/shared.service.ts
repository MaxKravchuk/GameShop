import { Injectable } from '@angular/core';
import { Observable, Subject } from "rxjs";

@Injectable({
    providedIn: 'root'
})
export class SharedService<T> {

    private sharedSubject: Subject<T> = new Subject<T>();

    private reloadSourceSubject: Subject<void> = new Subject<void>();

    reloadSource$: Observable<any> = this.reloadSourceSubject.asObservable();

    sendData(model: T): void {
        this.sharedSubject.next(model);
    }

    getData$(): Observable<T> {
        return this.sharedSubject.asObservable();
    }

    reloadSource(): void {
        this.reloadSourceSubject.next();
    }
}
