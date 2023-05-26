import { BaseModel } from "./BaseModel";

export interface Comment extends BaseModel {

    Name?: string;

    Body?: string;

    GameKey?: string;

    ParentId?: number;

    HasQuotation?: boolean;
}
