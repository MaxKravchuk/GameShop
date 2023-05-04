import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CartMainComponent } from "./components/cart-main/cart-main.component";
import { CartRoutingModule } from "./cart-routing.module";
import { MatButtonModule } from "@angular/material/button";


@NgModule({
    declarations: [
        CartMainComponent
    ],
    imports: [
        CommonModule,
        CartRoutingModule,
        MatButtonModule
    ]
})
export class CartModule {
}
