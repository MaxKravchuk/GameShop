import {ResourceModel} from "./resource-model";

export class Game extends ResourceModel<Game>{
  Key?:string;
  Name?:string;
  Description?:string;

  constructor(model?: Partial<Game>) {
    super(model);
  }
}
