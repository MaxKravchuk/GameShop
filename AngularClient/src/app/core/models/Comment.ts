import {ResourceModel} from "./resource-model";

export class Comment extends ResourceModel<Comment>{
  Name?:string;
  Body?:string;
  GameKey?:string;
  ParentId?:number;

  constructor(model?: Partial<Comment>) {
    super(model);
  }
}
