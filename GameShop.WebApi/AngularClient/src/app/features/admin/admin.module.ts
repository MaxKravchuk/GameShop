import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminRoutingModule } from "./admin-routing.module";
import { MatButtonModule } from "@angular/material/button";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { AdminMainComponent } from "./components/admin-main/admin-main.component";
import { UserDeleteEditComponent } from './components/user-delete-edit/user-delete-edit.component';
import { RoleDeleteEditComponent } from './components/role-delete-edit/role-delete-edit.component';
import { MatDialogModule } from "@angular/material/dialog";
import { UserAddComponent } from './components/user-add/user-add.component';
import { RoleAddComponent } from './components/role-add/role-add.component';


@NgModule({
    declarations: [
        AdminMainComponent,
        UserDeleteEditComponent,
        RoleDeleteEditComponent,
        UserAddComponent,
        RoleAddComponent
    ],
    imports: [
        CommonModule,
        AdminRoutingModule,
        MatButtonModule,
        FormsModule,
        MatFormFieldModule,
        MatInputModule,
        ReactiveFormsModule,
        MatDialogModule
    ]
})
export class AdminModule {
}
