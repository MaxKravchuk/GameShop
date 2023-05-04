import { Injectable } from '@angular/core';
import { PlatformType } from "../../models/PlatformType";
import { HttpClient } from "@angular/common/http";
import { catchError, Observable } from "rxjs";
import { UtilsService } from "../helpers/utilsService/utils-service";
import { map } from "rxjs/operators";

@Injectable({
    providedIn: 'root'
})
export class PlatformTypeService {

    private apiUrl: string = "/api/platformTypes/";

    constructor(
        private http: HttpClient,
        private utilsService: UtilsService) {
    }

    getAllPlatformTypes(): Observable<PlatformType[]> {
        return this.http.get<PlatformType[]>(`${this.apiUrl}getAll`)
            .pipe(
                map((platformTypes: PlatformType[]) => {
                    return platformTypes;
                    }
                ),
                catchError(err => {
                    console.log(err);
                    this.utilsService.openWithMessage(err.message);
                    return [];
                })
            );
    }
}
