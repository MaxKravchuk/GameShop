import { BaseModel } from "./BaseModel";
import { OrderDetail } from "./OrderDetail";

export interface Order extends BaseModel {

    CustomerNickName?: string;

    OrderedAt?: Date;

    Status?: string;

    OrderDetails?: OrderDetail[];
}
