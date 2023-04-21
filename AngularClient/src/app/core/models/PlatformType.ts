import {ResourceModel} from "./resource-model";

export class PlatformType extends ResourceModel<PlatformType>{
  Type?:string;

  constructor(model?: Partial<PlatformType>) {
    super(model);
  }
}
