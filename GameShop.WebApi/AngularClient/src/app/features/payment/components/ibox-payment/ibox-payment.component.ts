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
    ) {
        this.sum = this.router.getCurrentNavigation()?.extras.state?.['sum'];
    }

    ngOnInit(): void {
        const orderCreateDTO: CreateOrderModel ={
            CustomerId: this.accountNumber,
            OrderedAt: new Date().toISOString(),
            Strategy: 'iBox',
            IsPaymentSuccessful: true
        };

        this.paymentService.getOrderId(orderCreateDTO)
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
