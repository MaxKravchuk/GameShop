import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { VisaPaymentComponent } from "./components/visa-payment/visa-payment.component";
import { IboxPaymentComponent } from "./components/ibox-payment/ibox-payment.component";
import { AuthGuard } from "../../core/guards/auth/auth.guard";

const routes: Routes = [
    {
        path: 'visa',
        component: VisaPaymentComponent,
        canActivate: [AuthGuard]
    },
    {
        path: 'ibox',
        component: IboxPaymentComponent,
        canActivate: [AuthGuard]
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
