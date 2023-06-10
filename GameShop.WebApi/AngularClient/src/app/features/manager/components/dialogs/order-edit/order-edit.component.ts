import { Component, Inject, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from "@angular/forms";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { OrdersMainComponent } from "../../orders-main/orders-main.component";
import { Order } from "../../../../../core/models/Order";
import { OrderDetail } from "../../../../../core/models/OrderDetail";
import { OrderService } from "../../../../../core/services/orderService/order.service";

@Component({
  selector: 'app-order-edit',
  templateUrl: './order-edit.component.html',
  styleUrls: ['./order-edit.component.css']
})
export class OrderEditComponent implements OnInit {

    form!: FormGroup;

    order!: Order;

    statuses: string[] = ['Paid', 'Shipped'];

    isUpdatingStatus!: boolean;

    constructor(
        private formBuilder: FormBuilder,
        @Inject(MAT_DIALOG_DATA) private data : {order: Order, isUpdatingStatus: boolean},
        private dialogRef: MatDialogRef<OrdersMainComponent>,
        private orderService: OrderService,
    ) { }

    ngOnInit(): void {
        this.order = this.data.order;
        this.isUpdatingStatus = this.data.isUpdatingStatus;

        this.form = this.formBuilder.group({
            Status: [this.order.Status, Validators.required],
            OrderDetails: this.formBuilder.array([])
        });

        if (!this.isUpdatingStatus) {
            this.addOrderDetailFormGroup();
        }
    }

    get orderDetails(): FormArray {
        return this.form.get('OrderDetails') as FormArray;
    }

    onNoClick(): void {
        this.dialogRef.close(false);
    }

    onEditStatusClick(): void {
        const newOrder: Order = this.order;
        newOrder.Status = this.form.value.Status;

        this.orderService.updateOrderStatus(newOrder).subscribe({
            next: (): void => {
                this.dialogRef.close(true);
            }
        });
    }

    onEditDetailsClick(): void {
        const newOrder: Order = this.order;
        newOrder.OrderDetails = this.form.value.OrderDetails;

        this.orderService.updateOrderDetails(newOrder).subscribe({
            next: (): void => {
                this.dialogRef.close(true);
            }
        });
    }

    private addOrderDetailFormGroup(): void {

        this.order.OrderDetails!.forEach((detail: OrderDetail) => {
            const orderDetailFormGroup = this.formBuilder.group({
                GameKey: [detail.GameKey],
                Quantity: [detail.Quantity, Validators.required],
                Discount: [detail.Discount, Validators.required]
            });
            this.orderDetails.push(orderDetailFormGroup);
        });
    }
}
