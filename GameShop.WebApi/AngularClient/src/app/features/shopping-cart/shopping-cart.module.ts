import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CartMainComponent } from "./components/cart-main/cart-main.component";
import { ShoppingCartRoutingModule } from "./shopping-cart-routing.module";
import { MatButtonModule } from "@angular/material/button";


@NgModule({
    declarations: [
        CartMainComponent
    ],
    imports: [
        CommonModule,
        ShoppingCartRoutingModule,
        MatButtonModule
    ]
})
export class ShoppingCartModule {
}
