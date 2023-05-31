import { Component, OnInit } from '@angular/core';
import { UserService } from "../../../../core/services/userService/user.service";
import { User } from "../../../../core/models/User";
import { Role } from "../../../../core/models/Role";
import { RoleService } from "../../../../core/services/roleService/role.service";
import { forkJoin } from "rxjs";
import { MatDialog } from "@angular/material/dialog";
import { RoleDeleteEditComponent } from "../role-delete-edit/role-delete-edit.component";
import { UserDeleteEditComponent } from "../user-delete-edit/user-delete-edit.component";
import { UserAddComponent } from "../user-add/user-add.component";
import { RoleAddComponent } from "../role-add/role-add.component";

@Component({
  selector: 'app-admin-main',
  templateUrl: './admin-main.component.html',
  styleUrls: ['./admin-main.component.css']
})
export class AdminMainComponent implements OnInit {

    users: User[] = [];

    roles: Role[] = [];

    constructor(
        private userService: UserService,
        private roleService: RoleService,
        private dialog: MatDialog
    ) { }

    ngOnInit(): void {
        forkJoin([
            this.userService.getAllUsers(),
            this.roleService.getAllRoles()
            ]).subscribe(([users, roles]: [User[], Role[]]): void => {
            this.users = users;
            this.roles = roles;
        });
    }

    editDeleteUser(user: User): void {
        const dialogRef = this.dialog.open(UserDeleteEditComponent, {
            autoFocus: false,
            data: {
                user: user
            }
        });

        dialogRef.afterClosed().subscribe((requireReload:boolean): void => {
            if(requireReload) {
                this.ngOnInit();
            }
        });
    }

    editDeleteRole(role: Role): void {
        const dialogRef = this.dialog.open(RoleDeleteEditComponent, {
            autoFocus: false,
            data: {
                role: role
            }
        });

        dialogRef.afterClosed().subscribe((requireReload:boolean): void => {
            if(requireReload) {
                this.ngOnInit();
            }
        });
    }

    addUser(): void {
        const dialogRef = this.dialog.open(UserAddComponent, {
            autoFocus: false
        });

        dialogRef.afterClosed().subscribe((requireReload:boolean): void => {
            if(requireReload) {
                this.ngOnInit();
            }
        });
    }

    addRole(): void {
        const dialogRef = this.dialog.open(RoleAddComponent, {
            autoFocus: false
        });

        dialogRef.afterClosed().subscribe((requireReload:boolean): void => {
            if(requireReload) {
                this.ngOnInit();
            }
        });
    }
}
