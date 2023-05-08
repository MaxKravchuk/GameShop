import { Injectable } from '@angular/core';
import { CartItem } from "../../models/CartItem";
import { HttpClient } from "@angular/common/http";
import { catchError, Observable } from "rxjs";
import { UtilsService } from "../helpers/utilsService/utils-service";

@Injectable({
    providedIn: 'root'
})
export class CartService {

    private apiUrl: string = '/api/shoppingcart/';

    constructor(
        private http: HttpClient,
        private utilsService: UtilsService
    ) {}

    addToCart(cartItem: CartItem): Observable<CartItem> {
        return this.http.post<CartItem>(`${this.apiUrl}addToCart`, cartItem)
            .pipe(
                catchError(err => {
                    this.utilsService.handleError(err);
                    return [];
                })
            );
    }

    getCartItems(): Observable<CartItem[]> {
        return this.http.get<CartItem[]>(`${this.apiUrl}getAll`)
            .pipe(
                catchError(err => {
                    this.utilsService.handleError(err);
                    return [];
                })
            );
    }

    deleteItemFromCart(key: string): Observable<any> {
        return this.http.delete(`${this.apiUrl}delete/${key}`)
            .pipe(
                catchError(err => {
                    this.utilsService.handleError(err);
                    return [];
                })
            );
    }
}
