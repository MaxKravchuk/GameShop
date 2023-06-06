import { Component, OnInit } from '@angular/core';
import { CartItem } from "../../../../core/models/CartItem";
import { CartService } from "../../../../core/services/cartService/cart.service";
import { UtilsService } from "../../../../core/services/helpers/utilsService/utils-service";
import { PaymentService } from "../../../../core/services/paymentService/payment.service";
import { saveAs } from "file-saver";
import { NavigationExtras, Router } from "@angular/router";
import { PaymentCreateDTO } from "../../../../core/models/PaymentCreateDTO";

@Component({
  selector: 'app-order-main',
  templateUrl: './order-main.component.html',
  styleUrls: ['./order-main.component.css']
})
export class OrderMainComponent implements OnInit{

    cartItems: CartItem[] = [];

    totalQuantity?: number;

    totalPrice?: number;

    customerId?: number;

    currentOrderId?: number;

    constructor(
        private cartService: CartService,
        private utilsService: UtilsService,
        private paymentService: PaymentService,
        private router: Router
    ) {}

    ngOnInit(): void {
        this.customerId = history.state.customerId;
        this.currentOrderId = history.state.orderId;

        this.cartService.getCartItems(history.state.customerId!).subscribe(
            (data: CartItem[]): void => {
                this.cartItems = data;
                this.getTotal();
            }
        );
    }

    generateInvoice(): void {
        const paymentCreateDTO : PaymentCreateDTO = {
            OrderId: this.currentOrderId,
            Strategy: 'Bank'
        };

        this.paymentService.getInvoice(paymentCreateDTO)
            .subscribe(
                (blob: Blob): void => {
                    this.utilsService.openWithMessage('Invoice generated successfully.');
                    saveAs(blob, `${this.customerId}.bin`);
                    setTimeout(() => {
                        this.router.navigateByUrl('/').then(
                            (): void => {
                                this.utilsService
                                    .openWithMessage(`Your order has been successfully created.`);
                            }
                        );
                    }, 2000);
                });
    }

    redirectToiBox(): void {
        const navExtras: NavigationExtras = { state:
                {
                    sum: this.totalPrice,
                    orderId: this.currentOrderId,
                    customerId: this.customerId
                }
        };
        this.router.navigateByUrl('/payment/ibox', navExtras);
    }

    redirectToVisa(): void {
        const navExtras: NavigationExtras = { state: { sum: this.totalPrice, orderId: this.currentOrderId } };
        this.router.navigateByUrl('/payment/visa', navExtras);
    }

    private getTotal(): void {
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
