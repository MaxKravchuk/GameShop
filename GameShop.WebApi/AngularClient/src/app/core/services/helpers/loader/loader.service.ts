import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class LoaderService {

    private loadingSub: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);

    constructor() { }

    setLoading(loading: boolean): void {
        this.loadingSub.next(loading);
    }

    getLoading(): Observable<boolean> {
        return this.loadingSub.asObservable();
    }
}
