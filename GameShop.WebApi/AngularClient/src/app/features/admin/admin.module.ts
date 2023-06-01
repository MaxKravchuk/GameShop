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
import { MatOptionModule } from "@angular/material/core";
import { MatSelectModule } from "@angular/material/select";


@NgModule({
    declarations: [
        AdminMainComponent,
        UserDeleteEditComponent,
        RoleDeleteEditComponent,
    ],
    imports: [
        CommonModule,
        AdminRoutingModule,
        MatButtonModule,
        FormsModule,
        MatFormFieldModule,
        MatInputModule,
        ReactiveFormsModule,
        MatDialogModule,
        MatOptionModule,
        MatSelectModule
    ]
})
export class AdminModule {
}
