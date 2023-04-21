import {ResourceModel} from "./resource-model";

export class Genre extends ResourceModel<Genre>
{
  Name?:string;

  constructor(model?: Partial<Genre>) {
    super(model);
  }
}
