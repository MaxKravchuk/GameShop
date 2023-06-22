import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from "@angular/forms";
import { User } from "../../../../../core/models/User";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { AdminMainComponent } from "../../admin-main/admin-main.component";
import { UserService } from "../../../../../core/services/userService/user.service";
import { Role } from "../../../../../core/models/Role";
import { RoleService } from "../../../../../core/services/roleService/role.service";
import { UtilsService } from "../../../../../core/services/helpers/utilsService/utils-service";
import { Publisher } from "../../../../../core/models/Publisher";
import { PublisherService } from "../../../../../core/services/publisherService/publisher.service";
import { forkJoin } from "rxjs";

@Component({
  selector: 'app-user-crud',
  templateUrl: './user-crud.component.html',
  styleUrls: ['./user-crud.component.scss']
})
export class UserCrudComponent implements OnInit {

    form!: FormGroup;

    user!: User;

    isAdding: boolean = false;

    roles: Role[] = [];

    publishers: Publisher[] = [];

    isAddingPublisher: boolean = false;

    constructor(
        private formBuilder: FormBuilder,
        @Inject(MAT_DIALOG_DATA) private data : {user: User},
        private dialogRef: MatDialogRef<AdminMainComponent>,
        private roleService: RoleService,
        private userService: UserService,
        private publisherService: PublisherService,
        private utilsService: UtilsService
    ) { }

    ngOnInit(): void {
        this.form = this.formBuilder.group({
            NickName: ['', [Validators.required,Validators.minLength(1)]],
            Password: ['', [Validators.required,Validators.minLength(1)]],
            RoleId: [''],
        });

        forkJoin([this.roleService.getAllRoles(), this.publisherService.getAllPublishers()])
            .subscribe(([roles, publishers] : [Role[], Publisher[]]): void => {
                this.roles = roles;
                this.publishers = publishers;
            }
        );

        if (this.data.user == null) {
            this.isAdding = true;
            this.form.controls['RoleId'].setValidators(Validators.required);
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

    onRoleSelected(): void {
        const selectedRoleId = this.form.controls['RoleId'].value;
        const selectedRole: Role | undefined = this.roles.find((role: Role): boolean => role.Id === selectedRoleId);

        if (selectedRole && selectedRole.Name === 'Publisher') {
            this.isAddingPublisher = true;
            this.form.addControl('PublisherId', new FormControl('', Validators.required));
        } else {
            this.isAddingPublisher = false;
            this.form.removeControl('PublisherId');
        }
    }
}
