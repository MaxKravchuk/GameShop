import { Component, OnInit } from '@angular/core';
import { PaymentService } from "../../../../core/services/paymentService/payment.service";
import { CreateOrderModel } from "../../../../core/models/CreateOrderModel";
import { Router } from "@angular/router";
import { UtilsService } from "../../../../core/services/helpers/utilsService/utils-service";

@Component({
  selector: 'app-ibox-payment',
  templateUrl: './ibox-payment.component.html',
  styleUrls: ['./ibox-payment.component.css']
})
export class IboxPaymentComponent implements OnInit {

    accountNumber: number = 0;

    invoiceNumber?: number;

    sum?: number;

    constructor(
        private paymentService: PaymentService,
        private utilsService: UtilsService,
        private router: Router
    ) {}

    ngOnInit(): void {
        this.sum = history.state.sum;

        const createOrderModel: CreateOrderModel ={
            CustomerId: this.accountNumber,
            OrderedAt: new Date().toISOString(),
            Strategy: 'iBox'
        };

        this.paymentService.getOrderId(createOrderModel)
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
