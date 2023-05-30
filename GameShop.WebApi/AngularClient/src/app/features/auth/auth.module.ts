import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthRoutingModule } from "./auth-routing.module";
import { MatButtonModule } from "@angular/material/button";
import { AuthMainComponent } from "./components/auth-main/auth-main.component";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";


@NgModule({
    declarations: [
        AuthMainComponent
    ],
    imports: [
        CommonModule,
        AuthRoutingModule,
        MatButtonModule,
        FormsModule,
        MatFormFieldModule,
        MatInputModule,
        ReactiveFormsModule
    ]
})
export class AuthModule {
}
