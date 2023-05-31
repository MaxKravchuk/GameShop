import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from "@angular/forms";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { Role } from "../../../../core/models/Role";
import { AdminMainComponent } from "../admin-main/admin-main.component";
import { RoleService } from "../../../../core/services/roleService/role.service";

@Component({
  selector: 'app-role-delete-edit',
  templateUrl: './role-delete-edit.component.html',
  styleUrls: ['./role-delete-edit.component.css']
})
export class RoleDeleteEditComponent implements OnInit {

    form!: FormGroup;

    role!: Role;

    constructor(
        private formBuilder: FormBuilder,
        @Inject(MAT_DIALOG_DATA) private data : {role: Role},
        private dialogRef: MatDialogRef<AdminMainComponent>,
        private roleService: RoleService
    ) { }

    ngOnInit(): void {
        this.form = this.formBuilder.group({
            Name: [''],
        });

        this.role = this.data.role;
    }

    onNoClick(): void {
        this.dialogRef.close(false);
    }

    onEditClick(): void {
        const newRole: Role = this.form.value as Role;
        console.log(newRole);
    }

    onDeleteClick(): void {

    }
}
