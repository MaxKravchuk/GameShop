import { NgModule } from '@angular/core';
import { CommonModule, NgOptimizedImage } from '@angular/common';
import { OrderMainComponent } from "./components/order-main/order-main.component";
import { OrderRoutingModule } from "./order-routing.module";
import { MatButtonModule } from "@angular/material/button";



@NgModule({
  declarations: [
      OrderMainComponent
  ],
    imports: [
        CommonModule,
        OrderRoutingModule,
        MatButtonModule,
        NgOptimizedImage
    ]
})
export class OrderModule { }
