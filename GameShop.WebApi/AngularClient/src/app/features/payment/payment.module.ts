import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IboxPaymentComponent } from './components/ibox-payment/ibox-payment.component';
import { VisaPaymentComponent } from './components/visa-payment/visa-payment.component';
import { PaymentRoutingModule } from "./payment-routing.module";



@NgModule({
  declarations: [
      IboxPaymentComponent,
      VisaPaymentComponent
  ],
  imports: [
      CommonModule,
      PaymentRoutingModule
  ]
})
export class PaymentModule { }
