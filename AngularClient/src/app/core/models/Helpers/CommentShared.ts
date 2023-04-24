export class CommentShared{
  Name?:string;
  CommentId?:number;
  constructor(name?:string,commentId?:number) {
    this.Name = name;
    this.CommentId = commentId;
  }
}
