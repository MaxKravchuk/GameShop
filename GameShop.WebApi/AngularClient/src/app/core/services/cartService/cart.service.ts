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

    getCartItems(customerId: number): Observable<CartItem[]> {
        return this.http.get<CartItem[]>(`${this.apiUrl}getAll/${customerId}`)
            .pipe(
                catchError(err => {
                    this.utilsService.handleError(err);
                    return [];
                })
            );
    }

    deleteItemFromCart(customerId: number ,key: string): Observable<any> {
        return this.http.delete(`${this.apiUrl}delete/${customerId}-${key}`)
            .pipe(
                catchError(err => {
                    this.utilsService.handleError(err);
                    return [];
                })
            );
    }

    getNumberOfGamesInCart(customerId: number ,key: string): Observable<number> {
        return this.http.get<number>(`${this.apiUrl}numberOfGames/${customerId}-${key}`)
            .pipe(
                catchError(err => {
                    this.utilsService.handleError(err);
                    return [];
                })
            );
    }
}
