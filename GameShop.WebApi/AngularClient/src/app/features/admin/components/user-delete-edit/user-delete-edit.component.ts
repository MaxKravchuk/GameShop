import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from "@angular/forms";
import { User } from "../../../../core/models/User";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { AdminMainComponent } from "../admin-main/admin-main.component";
import { UserService } from "../../../../core/services/userService/user.service";

@Component({
  selector: 'app-user-delete-edit',
  templateUrl: './user-delete-edit.component.html',
  styleUrls: ['./user-delete-edit.component.css']
})
export class UserDeleteEditComponent implements OnInit {

    form!: FormGroup;

    user!: User;

    constructor(
        private formBuilder: FormBuilder,
        @Inject(MAT_DIALOG_DATA) private data : {user: User},
        private dialogRef: MatDialogRef<AdminMainComponent>,
        private userService: UserService
    ) { }

    ngOnInit(): void {
        this.form = this.formBuilder.group({
            NickName: [''],
        });

        this.user = this.data.user;
    }

    onNoClick(): void {
        this.dialogRef.close(false);
    }

    onEditClick(): void {

    }

    onDeleteClick(): void {

    }
}
