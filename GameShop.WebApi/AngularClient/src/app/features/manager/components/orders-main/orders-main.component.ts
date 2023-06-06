import { Component, OnInit } from '@angular/core';
import { OrderService } from "../../../../core/services/orderService/order.service";
import { Order } from "../../../../core/models/Order";
import { MatDialog } from "@angular/material/dialog";
import { OrderEditComponent } from "../dialogs/order-edit/order-edit.component";
import { switchMap } from "rxjs";
import { AuthService } from "../../../../core/services/authService/auth.service";

@Component({
  selector: 'app-orders-main',
  templateUrl: './orders-main.component.html',
  styleUrls: ['./orders-main.component.css']
})
export class OrdersMainComponent implements OnInit {

    orders: Order[] = [];

    constructor(
        private orderService: OrderService,
        private dialog: MatDialog,
        private authService: AuthService
    ) { }

    ngOnInit(): void {
        this.orderService.getAllOrders().subscribe((orders: Order[]): void => {
            this.orders = orders;
        });
    }

    onEdit(Id: number): void {
        this.orderService.getOrderById(Id).pipe(
            switchMap((order: Order) => {
                const dialogRef = this.dialog.open(OrderEditComponent, {
                    autoFocus: false,
                    data: {
                        order: order,
                    }
                });

                return dialogRef.afterClosed();
            })
        ).subscribe((requireReload: boolean): void => {
            if (requireReload) {
                this.ngOnInit();
            }
        });
    }
}
