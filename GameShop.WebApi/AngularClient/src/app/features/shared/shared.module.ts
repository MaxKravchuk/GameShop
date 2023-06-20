import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PaginationComponent } from './components/pagination/pagination.component';
import { MatButtonModule } from "@angular/material/button";
import { ReactiveFormsModule } from "@angular/forms";
import { IsInRoleDirective } from './directives/isInRole/is-in-role.directive';



@NgModule({
    declarations: [
        PaginationComponent,
        IsInRoleDirective
    ],
    exports: [
        PaginationComponent,
        IsInRoleDirective
    ],
    imports: [
        CommonModule,
        MatButtonModule,
        ReactiveFormsModule
    ]
})
export class SharedModule { }
