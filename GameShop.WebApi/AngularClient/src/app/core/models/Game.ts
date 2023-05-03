import { BaseModel } from "./BaseModel";
import { Genre } from "./Genre";
import { PlatformType } from "./PlatformType";
import { Publisher } from "./Publisher";

export interface Game extends BaseModel {

    Key?: string;

    Name?: string;

    Description?: string;

    Price?: number;

    UnitsInStock?: boolean;

    Discontinued?: boolean;

    Genres?: Genre[];

    PlatformTypes?: PlatformType[];

    PublisherReadDTO?: Publisher;
}
