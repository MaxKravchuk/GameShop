import { Component, Inject, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from "@angular/forms";
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

    detailsFormGroup!: FormGroup;

    order!: Order;

    statuses: string[] = ['Paid', 'Shipped'];

    constructor(
        private formBuilder: FormBuilder,
        @Inject(MAT_DIALOG_DATA) private data : {order: Order},
        private dialogRef: MatDialogRef<OrdersMainComponent>,
        private orderService: OrderService,
    ) { }

    ngOnInit(): void {
        this.order = this.data.order;

        this.form = this.formBuilder.group({
            Status: [this.order.Status, Validators.required],
            OrderDetails: this.formBuilder.array([])
        });

        this.addOrderDetailFormGroup();
    }

    get orderDetails(): FormArray {
        return this.form.get('OrderDetails') as FormArray;
    }

    updateOrderDetail(detail: OrderDetail): void {

    }

    deleteOrderDetail(detail: OrderDetail): void {

    }

    onNoClick(): void {
        this.dialogRef.close(false);
    }


    onEditClick(): void {
        const newOrder: Order = this.order;
        newOrder.Status = this.form.value.Status;
        newOrder.OrderDetails = this.form.value.OrderDetails;

        this.orderService.updateOrder(newOrder).subscribe((order: Order): void => {
            this.dialogRef.close(true);
        });
    }
    private addOrderDetailFormGroup() {

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
