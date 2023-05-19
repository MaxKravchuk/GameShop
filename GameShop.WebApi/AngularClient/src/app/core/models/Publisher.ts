import { BaseModel } from "./BaseModel";
import { Game } from "./Game";

export interface Publisher extends BaseModel {

    CompanyName?: string;

    Description?: string;

    HomePage?: string;

    GameReadListDTOs?: Game[];
}
