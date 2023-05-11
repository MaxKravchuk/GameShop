import { Component, OnInit } from '@angular/core';
import { CartItem } from "../../../../core/models/CartItem";
import { CartService } from "../../../../core/services/cartService/cart.service";
import { UtilsService } from "../../../../core/services/helpers/utilsService/utils-service";

@Component({
  selector: 'app-order-main',
  templateUrl: './order-main.component.html',
  styleUrls: ['./order-main.component.css']
})
export class OrderMainComponent implements OnInit{

    cartItems: CartItem[] = [];

    totalQuantity?: number;

    totalPrice?: number;

    constructor(
        private cartService: CartService,
        private utilsService: UtilsService
    ) {}

    ngOnInit(): void {
        this.cartService.getCartItems().subscribe(
            (data: CartItem[]) => {
                this.cartItems = data;
                this.getTotal();
            }
        );
    }

    private getTotal(){
        let totalPrice: number = 0;
        let totalQuantity: number = 0;
        for (let cartItem of this.cartItems) {
            totalPrice += cartItem.Quantity! * cartItem.GamePrice!;
            totalQuantity += cartItem.Quantity!;
        }
        this.totalPrice = totalPrice;
        this.totalQuantity = totalQuantity;
    }
}
