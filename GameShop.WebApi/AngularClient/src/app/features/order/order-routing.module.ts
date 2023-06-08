import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { OrderMainComponent } from "./components/order-main/order-main.component";
import { AuthGuard } from "../../core/guards/auth/auth.guard";

const routes: Routes = [
    {
        path: '',
        component: OrderMainComponent,
        canActivate: [AuthGuard]
    }
];

@NgModule({
    imports: [
        RouterModule.forChild(routes)
    ],
    exports: [RouterModule]
})
export class OrderRoutingModule {
}
