import { Component, OnInit } from '@angular/core';
import { CartItem } from "../../../../core/models/CartItem";
import { CartService } from "../../../../core/services/cartService/cart.service";
import { OrderService } from "../../../../core/services/orderService/order.service";
import { CreateOrderModel } from "../../../../core/models/CreateOrderModel";
import { NavigationExtras, Router } from "@angular/router";
import { AuthService } from "../../../../core/services/authService/auth.service";

@Component({
    selector: 'app-cart-main',
    templateUrl: './cart-main.component.html',
    styleUrls: ['./cart-main.component.scss']
})
export class CartMainComponent implements OnInit {

    cartItems: CartItem[] = [];

    totalPrice: number = 0;

    customerId!: number;

    constructor(
        private shoppingCartService: CartService,
        private orderService: OrderService,
        private router: Router,
        private authService: AuthService) {}

    ngOnInit(): void {
        this.customerId = this.authService.getUserId();
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
        this.shoppingCartService.deleteItemFromCart(this.customerId, gameKey).subscribe({
            next: (): void => {
                this.fetchCart();
            }
        });
    }

    createOrder(): void {
        const data : CreateOrderModel = {
            CustomerId: this.customerId,
            OrderedAt: new Date().toISOString(),
        };

        this.orderService.createOrder(data).subscribe({
            next: (data: number): void => {
                const navExtras: NavigationExtras = { state: { customerId: this.customerId, orderId: data } };
                this.router.navigateByUrl('/order', navExtras);
            }
        });
    }

    private fetchCart(): void {
        this.shoppingCartService.getCartItems(this.customerId).subscribe(
            (cartItems: CartItem[]): void => {
                this.cartItems = cartItems;
                this.getTotalPrice();
            }
        );
    }
}
