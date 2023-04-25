import {ResourceModel} from "./resource-model";
import {Genre} from "./Genre";
import {PlatformType} from "./PlatformType";
import {Publisher} from "./Publisher";

export class Game extends ResourceModel<Game>{
  Key?:string;
  Name?:string;
  Description?:string;
  Price?:number;
  UnitsInStock?:boolean;
  Discontinued?:boolean;
  Genres?: Genre[];
  PlatformTypes?: PlatformType[];
  PublisherReadDTO? : Publisher;

  constructor(model?: Partial<Game>) {
    super(model);
  }
}
