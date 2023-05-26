import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { UtilsService } from "../helpers/utilsService/utils-service";
import { CreateOrderModel } from "../../models/CreateOrderModel";
import { catchError, Observable } from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class OrderService {

    private apiUrl: string = "/api/orders";

    constructor(
        private http: HttpClient,
        private utilsService: UtilsService
    ) { }

    createOrder(orderCreateDTO: CreateOrderModel): Observable<number> {
        return this.http.post<number>(this.apiUrl, orderCreateDTO).pipe(
            catchError(err => {
                this.utilsService.handleError(err);
                return [];
            })
        );
    }
}
