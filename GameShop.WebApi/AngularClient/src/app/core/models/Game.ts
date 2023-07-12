import { BaseModel } from "./BaseModel";
import { Genre } from "./Genre";
import { PlatformType } from "./PlatformType";
import { Publisher } from "./Publisher";

export interface Game extends BaseModel {

    Key?: string;

    Name?: string;

    IsDeleted?: boolean;

    Description?: string;

    Price?: number;

    UnitsInStock?: number;

    Discontinued?: boolean;

    PhotoUrl?: string | null;

    Genres?: Genre[];

    PlatformTypes?: PlatformType[];

    PublisherReadDTO?: Publisher;
}
