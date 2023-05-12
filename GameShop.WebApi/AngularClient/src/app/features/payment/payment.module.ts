import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IboxPaymentComponent } from './components/ibox-payment/ibox-payment.component';
import { VisaPaymentComponent } from './components/visa-payment/visa-payment.component';
import { PaymentRoutingModule } from "./payment-routing.module";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { MatDatepickerModule } from "@angular/material/datepicker";
import { MatNativeDateModule } from "@angular/material/core";
import { MatButtonModule } from "@angular/material/button";



@NgModule({
  declarations: [
      IboxPaymentComponent,
      VisaPaymentComponent
  ],
    imports: [
        CommonModule,
        PaymentRoutingModule,
        FormsModule,
        MatFormFieldModule,
        MatInputModule,
        ReactiveFormsModule,
        MatDatepickerModule,
        MatNativeDateModule,
        MatButtonModule
    ]
})
export class PaymentModule { }
