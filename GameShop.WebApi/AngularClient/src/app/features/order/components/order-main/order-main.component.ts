import { Component, OnInit } from '@angular/core';
import { CartItem } from "../../../../core/models/CartItem";
import { CartService } from "../../../../core/services/cartService/cart.service";
import { UtilsService } from "../../../../core/services/helpers/utilsService/utils-service";
import { PaymentService } from "../../../../core/services/paymentService/payment.service";
import { CreateOrderModel } from "../../../../core/models/CreateOrderModel";
import { saveAs } from "file-saver";
import { ActivatedRoute, NavigationExtras, Router } from "@angular/router";

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

    constructor(
        private cartService: CartService,
        private utilsService: UtilsService,
        private paymentService: PaymentService,
        private router: Router,
        private activeRoute: ActivatedRoute
    ) {}

    ngOnInit(): void {
        this.customerId = Number(this.activeRoute.snapshot.paramMap.get('Key'));
        this.cartService.getCartItems().subscribe(
            (data: CartItem[]): void => {
                this.cartItems = data;
                this.getTotal();
            }
        );
    }

    generateInvoice(): void {
        const orderCreateDTO : CreateOrderModel = {
            CustomerId: this.customerId,
            OrderedAt: new Date().toISOString(),
            Strategy: 'Bank'
        };

        this.paymentService.getInvoice(orderCreateDTO)
            .subscribe(
                (blob: Blob): void => {
                    this.utilsService.openWithMessage('Invoice generated successfully.');
                    saveAs(blob, `${orderCreateDTO.CustomerId}.bin`);
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
        const navExtras: NavigationExtras = { state: { sum: this.totalPrice } };
        this.router.navigateByUrl('/payment/ibox', navExtras);
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
