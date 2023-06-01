import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { User } from "../../../../core/models/User";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { AdminMainComponent } from "../admin-main/admin-main.component";
import { UserService } from "../../../../core/services/userService/user.service";
import { Role } from "../../../../core/models/Role";
import { RoleService } from "../../../../core/services/roleService/role.service";
import { UtilsService } from "../../../../core/services/helpers/utilsService/utils-service";

@Component({
  selector: 'app-user-delete-edit',
  templateUrl: './user-delete-edit.component.html',
  styleUrls: ['./user-delete-edit.component.css']
})
export class UserDeleteEditComponent implements OnInit {

    form!: FormGroup;

    user!: User;

    isAdding: boolean = false;

    roles: Role[] = [];

    constructor(
        private formBuilder: FormBuilder,
        @Inject(MAT_DIALOG_DATA) private data : {user: User},
        private dialogRef: MatDialogRef<AdminMainComponent>,
        private roleService: RoleService,
        private userService: UserService,
        private utilsService: UtilsService
    ) { }

    ngOnInit(): void {
        this.form = this.formBuilder.group({
            NickName: ['', Validators.minLength(1)],
            Password: ['', Validators.minLength(1)],
            RoleId: [''],
        });

        this.roleService.getAllRoles().subscribe((roles: Role[]): void => {
            this.roles = roles;
        });

        if (this.data.user == null) {
            this.isAdding = true;
        }
        else {
            this.user = this.data.user;
            console.log(this.user);
            this.form.controls['NickName'].disable();
            this.form.controls['Password'].disable();
        }
    }

    onNoClick(): void {
        this.dialogRef.close(false);
    }

    onEditClick(): void {
        const newUser: User = this.form.value as User;
        newUser.Id = this.user.Id;
         this.userService.updateUser(newUser).subscribe((user: User): void => {
             this.utilsService.openWithMessage("User updated successfully!");
             this.dialogRef.close(true);
         });
    }

    onDeleteClick(): void {
        this.userService.deleteUser(this.user.Id!).subscribe((user: User): void => {
            this.utilsService.openWithMessage("User deleted successfully!");
            this.dialogRef.close(true);
        });
    }

    onSaveClick(): void {
        const newUser: User = this.form.value as User;
        this.userService.createUserWithRole(newUser).subscribe((user: User): void => {
           this.utilsService.openWithMessage("User created successfully!");
           this.dialogRef.close(true);
        });
    }
}
