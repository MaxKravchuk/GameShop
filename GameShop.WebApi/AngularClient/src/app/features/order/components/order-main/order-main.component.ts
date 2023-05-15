import { Component, OnInit } from '@angular/core';
import { CartItem } from "../../../../core/models/CartItem";
import { CartService } from "../../../../core/services/cartService/cart.service";
import { UtilsService } from "../../../../core/services/helpers/utilsService/utils-service";
import { PaymentService } from "../../../../core/services/paymentService/payment.service";
import { CreateOrderModel } from "../../../../core/models/CreateOrderModel";
import { saveAs } from "file-saver";
import { SharedService } from "../../../../core/services/helpers/sharedService/shared.service";
import { Router } from "@angular/router";

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
        private utilsService: UtilsService,
        private paymentService: PaymentService,
        private sharedService: SharedService<number>,
        private router: Router
    ) {}

    ngOnInit(): void {
        this.cartService.getCartItems().subscribe(
            (data: CartItem[]) => {
                this.cartItems = data;
                this.getTotal();
            }
        );
    }

    generateInvoice(): void {
        const orderCreateDTO : CreateOrderModel = {
            CustomerId: 0,
            OrderedAt: new Date().toISOString(),
            Strategy: 'Bank',
            IsPaymentSuccessful: true
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

    redirectToiBox() {
        setTimeout(() => {
            this.sharedService.sendData(this.totalPrice!);
        }, 10);
        this.router.navigateByUrl('/payment/ibox');
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
