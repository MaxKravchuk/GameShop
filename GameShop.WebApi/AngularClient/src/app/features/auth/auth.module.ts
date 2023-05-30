import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthRoutingModule } from "./auth-routing.module";
import { MatButtonModule } from "@angular/material/button";
import { AuthMainComponent } from "./components/auth-main/auth-main.component";


@NgModule({
    declarations: [
        AuthMainComponent
    ],
    imports: [
        CommonModule,
        AuthRoutingModule,
        MatButtonModule
    ]
})
export class AuthModule {
}
