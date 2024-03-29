import { BaseModel } from "./BaseModel";

export interface Genre extends BaseModel {

    Name?: string;

    ParentGenreId?: number | null;
}
