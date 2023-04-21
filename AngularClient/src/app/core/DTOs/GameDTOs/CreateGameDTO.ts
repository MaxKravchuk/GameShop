import {ResourceModel} from "../../models/resource-model";

export class CreateGameDTO extends ResourceModel<CreateGameDTO>{
  Description?: string;
  GenresId? :number[];
  PlatformTypeId?: number[];
  Key?: string;
  Name?: string;

  constructor(model?: Partial<CreateGameDTO>) {
    super(model);
  }
}
