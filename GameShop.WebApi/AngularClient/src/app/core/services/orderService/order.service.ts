import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { UtilsService } from "../helpers/utilsService/utils-service";
import { CreateOrderModel } from "../../models/CreateOrderModel";
import { catchError, Observable } from "rxjs";
import { Order } from "../../models/Order";

@Injectable({
  providedIn: 'root'
})
export class OrderService {

    private apiUrl: string = "/api/orders/";

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

    getAllOrders(): Observable<Order[]> {
        return this.http.get<Order[]>(`${this.apiUrl}getAll`).pipe(
            catchError(err => {
                this.utilsService.handleError(err);
                return [];
            })
        );
    }

    getOrderById(id: number): Observable<Order> {
        return this.http.get<Order>(`${this.apiUrl}getById/${id}`).pipe(
            catchError(err => {
                this.utilsService.handleError(err);
                return [];
            })
        );
    }

    updateOrder(order: Order): Observable<Order> {
        return this.http.put<Order>(this.apiUrl, order).pipe(
            catchError(err => {
                this.utilsService.handleError(err);
                return [];
            })
        );
    }
}
