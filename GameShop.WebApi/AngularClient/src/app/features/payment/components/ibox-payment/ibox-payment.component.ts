import { Component, OnInit } from '@angular/core';
import { PaymentService } from "../../../../core/services/paymentService/payment.service";
import { Router } from "@angular/router";
import { UtilsService } from "../../../../core/services/helpers/utilsService/utils-service";
import { PaymentCreateDTO } from "../../../../core/models/PaymentCreateDTO";

@Component({
  selector: 'app-ibox-payment',
  templateUrl: './ibox-payment.component.html',
  styleUrls: ['./ibox-payment.component.scss']
})
export class IboxPaymentComponent implements OnInit {

    accountNumber?: number;

    invoiceNumber?: number;

    sum?: number;

    currentOrderId?: number;

    constructor(
        private paymentService: PaymentService,
        private utilsService: UtilsService,
        private router: Router
    ) {}

    ngOnInit(): void {
        this.sum = history.state.sum;
        this.currentOrderId = history.state.orderId;
        this.accountNumber = history.state.customerId;

        const paymentCreateDTO: PaymentCreateDTO ={
            OrderId: this.currentOrderId,
            Strategy: 'iBox'
        };

        this.paymentService.getOrderId(paymentCreateDTO)
            .subscribe(
                (data: number): void => {
                    this.invoiceNumber = data;
                    this.utilsService.openWithMessage('Payment successful.');
                }
            );
    }

    back(): void{
        this.router.navigateByUrl('/');
    }
}
