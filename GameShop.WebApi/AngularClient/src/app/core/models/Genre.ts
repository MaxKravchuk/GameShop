import { BaseModel } from "./BaseModel";

export interface Genre extends BaseModel {

    Name?: string;

    ParentId?: number;

    SubGenres?: Genre[];
}
