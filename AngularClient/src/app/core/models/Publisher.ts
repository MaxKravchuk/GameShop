import {ResourceModel} from "./resource-model";
import {Game} from "./Game";

export class Publisher extends ResourceModel<Publisher>
{
  CompanyName? : string;
  Description? : string;
  HomePage? : string;
  GameReadListDTOs? : Game[];

  constructor(model?: Partial<Publisher>) {
    super(model);
  }
}
