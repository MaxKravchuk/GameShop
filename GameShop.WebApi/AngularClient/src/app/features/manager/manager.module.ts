import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ManagerMainComponent } from './components/manager-main/manager-main.component';
import { MatButtonModule } from "@angular/material/button";
import { RouterLink } from "@angular/router";



@NgModule({
  declarations: [
    ManagerMainComponent
  ],
    imports: [
        CommonModule,
        MatButtonModule,
        RouterLink
    ]
})
export class ManagerModule { }
