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

    constructor(private shoppingCartService: ShoppingCartService) {}

    ngOnInit(): void {
        this.updateCart();
    }

    getTotalPrice(): number {
        let totalPrice: number = 0;
        for (let cartItem of this.cartItems) {
            totalPrice += cartItem.Quantity! * cartItem.GamePrice!;
        }
        return totalPrice;
    }

    removeFromCart(gameKey: string) {
        this.shoppingCartService.deleteItemFromCart(gameKey).subscribe({
            next: () => {
                this.updateCart();
            }
        });
    }

    private updateCart() {
        this.shoppingCartService.getCartItems().subscribe(
            (cartItems: CartItem[]) => {
                this.cartItems = cartItems;
            }
        );
    }
}
