import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { VisaPaymentComponent } from "./components/visa-payment/visa-payment.component";
import { IboxPaymentComponent } from "./components/ibox-payment/ibox-payment.component";

const routes: Routes = [
    {
        path: 'visa',
        component: VisaPaymentComponent
    },
    {
        path: 'ibox',
        component: IboxPaymentComponent
    }
];

@NgModule({
    imports: [
        RouterModule.forChild(routes)
    ],
    exports: [RouterModule]
})
export class PaymentRoutingModule {
}
