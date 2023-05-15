import { Component, OnDestroy, OnInit } from '@angular/core';
import { PaymentService } from "../../../../core/services/paymentService/payment.service";
import { CreateOrderModel } from "../../../../core/models/CreateOrderModel";
import { Subject, Subscription } from "rxjs";
import { SharedService } from "../../../../core/services/helpers/sharedService/shared.service";
import { Router } from "@angular/router";
import { UtilsService } from "../../../../core/services/helpers/utilsService/utils-service";

@Component({
  selector: 'app-ibox-payment',
  templateUrl: './ibox-payment.component.html',
  styleUrls: ['./ibox-payment.component.css']
})
export class IboxPaymentComponent implements OnInit, OnDestroy {

    accountNumber = 0;

    invoiceNumber?: number;

    sum?: number;

    private GetDataSub: Subscription = new Subscription();

    constructor(
        private paymentService: PaymentService,
        private utilsService: UtilsService,
        private sharedService: SharedService<number>,
        private router: Router) { }

    ngOnInit(): void {
        this.GetDataSub = this.sharedService.getData$().subscribe(
            (data: number): void => {
                console.log(data);
                this.sum = data;
            }
        );
        const orderCreateDTO: CreateOrderModel ={
            CustomerId: this.accountNumber,
            OrderedAt: new Date().toISOString(),
            Strategy: 'iBox',
            IsPaymentSuccessful: true
        };
        this.paymentService.getOrderId(orderCreateDTO)
            .subscribe(
                (data: number) => {
                    console.log(data);
                    this.invoiceNumber = data;
                    setTimeout(() => {
                        this.router.navigateByUrl('/').then(
                            (): void => {
                                this.utilsService
                                    .openWithMessage(`Your order has been successfully created.`);
                            }
                        );
                    }, 2000);
                }
            )
    }

    ngOnDestroy(): void {
        this.GetDataSub.unsubscribe();
    }
}
