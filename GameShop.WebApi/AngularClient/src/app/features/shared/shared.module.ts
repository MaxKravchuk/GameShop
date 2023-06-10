import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PaginationComponent } from './components/pagination/pagination.component';
import { MatButtonModule } from "@angular/material/button";
import { ReactiveFormsModule } from "@angular/forms";



@NgModule({
    declarations: [
        PaginationComponent
    ],
    exports: [
        PaginationComponent
    ],
    imports: [
        CommonModule,
        MatButtonModule,
        ReactiveFormsModule
    ]
})
export class SharedModule { }
