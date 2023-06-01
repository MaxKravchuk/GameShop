import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PlatformRoutingModule } from "./platform-routing.module";
import { MatButtonModule } from "@angular/material/button";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { PlatformCreateComponent } from './components/platform-create/platform-create.component';


@NgModule({
    declarations: [
        PlatformCreateComponent
    ],
    imports: [
        CommonModule,
        PlatformRoutingModule,
        MatButtonModule,
        FormsModule,
        MatFormFieldModule,
        MatInputModule,
        ReactiveFormsModule
    ]
})
export class PlatformModule {
}
