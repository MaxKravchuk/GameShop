import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminRoutingModule } from "./admin-routing.module";
import { MatButtonModule } from "@angular/material/button";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { AdminMainComponent } from "./components/admin-main/admin-main.component";
import { UserCrudComponent } from './components/dialogs/user-crud/user-crud.component';
import { RoleCrudComponent } from './components/dialogs/role-crud/role-crud.component';
import { MatDialogModule } from "@angular/material/dialog";
import { MatOptionModule } from "@angular/material/core";
import { MatSelectModule } from "@angular/material/select";


@NgModule({
    declarations: [
        AdminMainComponent,
        UserCrudComponent,
        RoleCrudComponent,
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
