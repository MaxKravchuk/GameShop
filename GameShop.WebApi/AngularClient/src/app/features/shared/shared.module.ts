import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PaginationComponent } from './components/pagination/pagination.component';
import { MatButtonModule } from "@angular/material/button";
import { ReactiveFormsModule } from "@angular/forms";
import { IsInRoleDirective } from './directives/isInRole/is-in-role.directive';
import { LoaderComponent } from './components/loader/loader.component';



@NgModule({
    declarations: [
        PaginationComponent,
        IsInRoleDirective,
        LoaderComponent
    ],
    exports: [
        PaginationComponent,
        IsInRoleDirective,
        LoaderComponent
    ],
    imports: [
        CommonModule,
        MatButtonModule,
        ReactiveFormsModule
    ]
})
export class SharedModule { }
