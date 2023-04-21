export abstract class ResourceModel<T> {
  public Id? :number;

  protected constructor(model?: Partial<T>) {
    if (model){
      Object.assign(this, model);
    }
  }
}
