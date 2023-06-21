import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { CartMainComponent } from "./components/cart-main/cart-main.component";
import { AuthGuard } from "../../core/guards/auth/auth.guard";

const routes: Routes = [
    {
        path: 'viewcart',
        component: CartMainComponent,
        canActivate: [AuthGuard]
    }
];

@NgModule({
    imports: [
        RouterModule.forChild(routes)
    ],
    exports: [RouterModule]
})
export class CartRoutingModule {
}
