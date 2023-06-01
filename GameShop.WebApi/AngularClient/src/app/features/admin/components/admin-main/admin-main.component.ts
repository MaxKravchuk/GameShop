import { Component, OnInit } from '@angular/core';
import { UserService } from "../../../../core/services/userService/user.service";
import { User } from "../../../../core/models/User";
import { Role } from "../../../../core/models/Role";
import { RoleService } from "../../../../core/services/roleService/role.service";
import { forkJoin } from "rxjs";
import { MatDialog } from "@angular/material/dialog";
import { RoleCrudComponent } from "../dialogs/role-crud/role-crud.component";
import { UserCrudComponent } from "../dialogs/user-crud/user-crud.component";

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

    addUser(): void {
        const dialogRef = this.dialog.open(UserCrudComponent, {
            autoFocus: false,
            data: {
                user: null
            }
        });

        dialogRef.afterClosed().subscribe((requireReload:boolean): void => {
            if(requireReload) {
                this.ngOnInit();
            }
        });
    }

    editDeleteUser(user: User): void {
        const dialogRef = this.dialog.open(UserCrudComponent, {
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

    addRole(): void {
        const dialogRef = this.dialog.open(RoleCrudComponent, {
            autoFocus: false,
            data: {
                role: null
            }
        });

        dialogRef.afterClosed().subscribe((requireReload:boolean): void => {
            if(requireReload) {
                this.ngOnInit();
            }
        });
    }

    editDeleteRole(role: Role): void {
        const dialogRef = this.dialog.open(RoleCrudComponent, {
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
}
