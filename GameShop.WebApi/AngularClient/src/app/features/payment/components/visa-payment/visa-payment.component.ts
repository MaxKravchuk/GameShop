import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { PaymentService } from "../../../../core/services/paymentService/payment.service";
import { UtilsService } from "../../../../core/services/helpers/utilsService/utils-service";
import { CreateOrderModel } from "../../../../core/models/CreateOrderModel";
import { Router } from "@angular/router";

@Component({
  selector: 'app-visa-payment',
  templateUrl: './visa-payment.component.html',
  styleUrls: ['./visa-payment.component.css']
})
export class VisaPaymentComponent implements OnInit{

    form!: FormGroup;

    constructor(
        private formBuilder: FormBuilder,
        private paymentService: PaymentService,
        private utilsService: UtilsService,
        private router: Router) {}

    ngOnInit(): void {
        this.form = this.formBuilder.group({
            CartHolderName: ['', Validators.required],
            CardNumber: ['', [Validators.required, Validators.minLength(16), Validators.pattern('^[0-9]*$')]],
            DateOfExpiry: ['', [Validators.required, Validators.pattern('^[0-9]{4}-[0-9]{2}$')]],
            CVV2CVC2: ['', [Validators.required, Validators.minLength(3), Validators.pattern('^[0-9]*$')]],
        });
    }

    onPay(): void {
        if (!this.form.valid) {
            this.utilsService.openWithMessage('Please fill all the fields');
        }
        const createOrderService: CreateOrderModel = {
            CustomerId: 0,
            OrderedAt: new Date().toISOString(),
            IsPaymentSuccessful: true,
            Strategy: 'Visa'
        }
        this.paymentService.getOrderId(createOrderService).subscribe(
            (data: number): void => {
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
}
