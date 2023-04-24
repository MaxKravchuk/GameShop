import {Component, Inject, Input, OnInit} from '@angular/core';
import {FormBuilder, FormControl, Validators} from "@angular/forms";
import {CommentService} from "../../../../core/services/commentService/comment.service";
import {Comment} from "../../../../core/models/Comment";
import {SharedCommentService} from "../../../../core/services/commentService/shared/shared-comment.service";
import {CommentShared} from "../../../../core/models/Helpers/CommentShared";

@Component({
  selector: 'app-create-comment',
  templateUrl: './create-comment.component.html',
  styleUrls: ['./create-comment.component.css']
})
export class CreateCommentComponent implements OnInit{
  ngOnInit(): void {
  }

  receivedData: CommentShared = new CommentShared("", undefined);
  @Input() gameKey?: string;
  constructor(
    @Inject(FormBuilder) private formBuilder: FormBuilder,
    private commentService: CommentService,
    private sharedService: SharedCommentService) {
    this.sharedService.getData().subscribe(data => {
      if(this.receivedData.Name == data.Name && this.receivedData.CommentId == data.CommentId){
        this.receivedData.Name = "";
        this.receivedData.CommentId = undefined;
      }
      else{
        this.receivedData = data;
      }
    });
  }

  form = this.formBuilder.group({
    Name: new FormControl("",Validators.required),
    Body: new FormControl("",Validators.required)
  });

  onSaveForm() {
    if(this.form.valid){
      const data:Comment = this.form.value as Comment;
      data.GameKey = this.gameKey;
      data.ParentId = this.receivedData.CommentId;
      if (this.receivedData?.Name != "" && this.receivedData?.CommentId != undefined){
        data.Body = '['+this.receivedData.Name+']'+ data.Body;
        data.ParentId = this.receivedData.CommentId;
      }
      this.commentService.createComment(data).subscribe();
      console.log(data);
      this.sharedService.reloadComments();
    }
  }
}
