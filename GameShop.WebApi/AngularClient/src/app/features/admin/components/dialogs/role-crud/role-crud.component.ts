import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from "@angular/forms";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { Role } from "../../../../../core/models/Role";
import { AdminMainComponent } from "../../admin-main/admin-main.component";
import { RoleService } from "../../../../../core/services/roleService/role.service";
import { UtilsService } from "../../../../../core/services/helpers/utilsService/utils-service";

@Component({
  selector: 'app-role-crud',
  templateUrl: './role-crud.component.html',
  styleUrls: ['./role-crud.component.scss']
})
export class RoleCrudComponent implements OnInit {

    form!: FormGroup;

    role!: Role;

    isAdding: boolean = false;

    constructor(
        private formBuilder: FormBuilder,
        @Inject(MAT_DIALOG_DATA) private data : {role: Role},
        private dialogRef: MatDialogRef<AdminMainComponent>,
        private roleService: RoleService,
        private utilsService: UtilsService
    ) { }

    ngOnInit(): void {
        this.form = this.formBuilder.group({
            Name: [''],
        });

        if (this.data.role == null) {
            this.isAdding = true;
        }
        else {
            this.role = this.data.role;
        }
    }

    onNoClick(): void {
        this.dialogRef.close(false);
    }

    onDeleteClick(): void {
        this.roleService.deleteRole(this.role.Id!).subscribe({
            next: (): void => {
                this.utilsService.openWithMessage("Role deleted successfully!");
                this.dialogRef.close(true);
            },
        });
    }

    onSaveClick(): void {
        const newRole: Role = this.form.value as Role;
        this.roleService.createRole(newRole).subscribe({
            next: (): void => {
                this.utilsService.openWithMessage("Role created successfully!");
                this.dialogRef.close(true);
            }
        });
    }
}
