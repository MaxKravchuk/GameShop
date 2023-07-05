import { Component, OnInit } from '@angular/core';
import { UserService } from "../../../../core/services/userService/user.service";
import { User } from "../../../../core/models/User";
import { Role } from "../../../../core/models/Role";
import { RoleService } from "../../../../core/services/roleService/role.service";
import { MatDialog } from "@angular/material/dialog";
import { RoleCrudComponent } from "../dialogs/role-crud/role-crud.component";
import { UserCrudComponent } from "../dialogs/user-crud/user-crud.component";
import { PagedList } from "../../../../core/models/PagedList";

@Component({
  selector: 'app-admin-main',
  templateUrl: './admin-main.component.html',
  styleUrls: ['./admin-main.component.scss']
})
export class AdminMainComponent implements OnInit {

    users?: User[] = [];

    roles?: Role[] = [];

    pageSizeRole!: number;

    pageSizeUser!: number;

    totalCountRoles!: number;

    totalCountUsers!: number;

    pageIndexRole!: number;

    pageIndexUser!: number;

    HasNextRole!: boolean;

    HasNextUser!: boolean;

    HasPreviousRole!: boolean;

    HasPreviousUser!: boolean;

    constructor(
        private userService: UserService,
        private roleService: RoleService,
        private dialog: MatDialog
    ) { }

    ngOnInit(): void {
        this.updateRoles();
        this.updateUsers();
    }

    pageSizeChangeRole(value: number): void {
        this.pageSizeRole = value;
        this.pageIndexRole = 1;
        this.updateRoles();
    }

    pageSizeChangeUser(value: number): void {
        this.pageSizeUser = value;
        this.pageIndexUser = 1;
        this.updateUsers();
    }

    pageIndexChangeRole(value: number): void {
        this.pageIndexRole = value;
        this.updateRoles();
    }

    pageIndexChangeUser(value: number): void {
        this.pageIndexUser = value;
        this.updateUsers();
    }

    updateRoles(): void {
        let roleParams = {
            pageNumber: this.pageIndexRole,
            pageSize: this.pageSizeRole
        };
        this.roleService.getAllRolesPaged(roleParams).subscribe((roles: PagedList<Role>): void => {
            this.roles = roles.Entities;
            this.HasNextRole = roles.HasNext;
            this.HasPreviousRole = roles.HasPrevious;
            this.totalCountRoles = roles.TotalCount;
        });
    }

    updateUsers(): void {
        let userParams = {
            pageNumber: this.pageIndexUser,
            pageSize: this.pageSizeUser
        };
        this.userService.getAllUsersPaged(userParams).subscribe((users: PagedList<User>): void => {
            this.users = users.Entities;
            this.HasNextUser = users.HasNext;
            this.HasPreviousUser = users.HasPrevious;
            this.totalCountUsers = users.TotalCount;
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
