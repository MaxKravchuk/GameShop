import { Component, OnInit } from '@angular/core';
import { CartItem } from "../../../../core/models/CartItem";
import { ShoppingCartService } from "../../../../core/services/cartService/shopping-cart.service";

@Component({
    selector: 'app-cart-main',
    templateUrl: './cart-main.component.html',
    styleUrls: ['./cart-main.component.css']
})
export class CartMainComponent implements OnInit {

    cartItems: CartItem[] = [];

    totalPrice: number = 0;

    constructor(private shoppingCartService: ShoppingCartService) {}

    ngOnInit(): void {
        this.fetchCart();
    }

    getTotalPrice(): void {
        let totalPrice: number = 0;
        for (let cartItem of this.cartItems) {
            totalPrice += cartItem.Quantity! * cartItem.GamePrice!;
        }
        this.totalPrice = totalPrice;
    }

    removeFromCart(gameKey: string): void {
        this.shoppingCartService.deleteItemFromCart(gameKey).subscribe({
            next: (): void => {
                this.fetchCart();
            }
        });
    }

    private fetchCart(): void {
        this.shoppingCartService.getCartItems().subscribe(
            (cartItems: CartItem[]): void => {
                this.cartItems = cartItems;
                this.getTotalPrice();
            }
        );
    }
}
