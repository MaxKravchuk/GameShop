import { BaseModel } from "./BaseModel";

export interface OrderDetail extends BaseModel {

    GameKey?: string;

    Quantity?: number;

    Discount?: number;
}
