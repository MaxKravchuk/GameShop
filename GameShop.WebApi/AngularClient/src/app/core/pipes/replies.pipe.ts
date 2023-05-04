import { Pipe, PipeTransform } from '@angular/core';
import { Comment } from "../models/Comment";

@Pipe({
  name: 'repliesPipe'
})
export class RepliesPipe implements PipeTransform {

    transform(comments: Comment[], commentId: number): Comment[] {
        return comments.filter((comment: Comment) => comment.ParentId == commentId);
    }
}
