import {NgModule} from "@angular/core";
import {RouterModule, Routes} from "@angular/router";
import {CartMainComponent} from "./components/cart-main/cart-main.component";

const routes: Routes = [
  {
    path:'viewcart',
    component: CartMainComponent
  }
];

@NgModule({
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class ShoppingCartRoutingModule { }
