import { BaseModel } from "./BaseModel";

export interface CreateGameModel extends BaseModel {

    Description?: string;

    GenresId?: number[];

    PlatformTypeId?: number[];

    Key?: string;

    Name?: string;
}
