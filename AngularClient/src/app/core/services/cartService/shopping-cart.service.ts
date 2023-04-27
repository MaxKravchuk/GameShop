import { Injectable } from '@angular/core';
import {CartItem} from "../../models/CartItem";
import {HttpClient, HttpErrorResponse} from "@angular/common/http";
import {catchError, Observable, throwError} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class ShoppingCartService{

  private apiUrl = '/api/shoppingcart';

  constructor(private http: HttpClient) { }

  addToCart(cartItem: CartItem): Observable<any> {
    return this.http.post<CartItem>(`${this.apiUrl}/addToCart`, cartItem)
      .pipe(
        catchError(this.handleError)
      );
  }

  getCartItems(): Observable<CartItem[]> {
    return this.http.get<CartItem[]>(this.apiUrl)
      .pipe(
        catchError(this.handleError)
      );
  }

  deleteItemFromCart(key: string){
    return this.http.delete(`${this.apiUrl}/delete/${key}`).pipe(
      catchError(this.handleError)
    );
  }

  private handleError(error: HttpErrorResponse): Observable<never> {
    console.error(`HTTP error: ${error.status}`);
    console.error(error);
    return throwError('Something bad happened; please try again later.');
  }
}
