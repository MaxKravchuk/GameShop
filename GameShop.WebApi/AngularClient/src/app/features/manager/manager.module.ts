import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ManagerMainComponent } from './components/manager-main/manager-main.component';
import { MatButtonModule } from "@angular/material/button";
import { RouterLink } from "@angular/router";
import { GenreCrudComponent } from './components/dialogs/genre-crud/genre-crud.component';
import { MatDialogModule } from "@angular/material/dialog";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { ReactiveFormsModule } from "@angular/forms";
import { GameModule } from "../game/game.module";
import { ManagerRoutingModule } from "./manager-routing.module";
import { MatOptionModule } from "@angular/material/core";
import { MatSelectModule } from "@angular/material/select";
import { PlatformTypeCrudComponent } from './components/dialogs/platform-type-crud/platform-type-crud.component';
import { PublisherCrudComponent } from './components/dialogs/publisher-crud/publisher-crud.component';
import { GameCrudComponent } from './components/dialogs/game-crud/game-crud.component';
import { OrdersMainComponent } from './components/orders-main/orders-main.component';
import { OrderEditComponent } from './components/dialogs/order-edit/order-edit.component';



@NgModule({
    declarations: [
        ManagerMainComponent,
        GenreCrudComponent,
        PlatformTypeCrudComponent,
        PublisherCrudComponent,
        GameCrudComponent,
        OrdersMainComponent,
        OrderEditComponent
    ],
    imports: [
        ManagerRoutingModule,
        CommonModule,
        MatButtonModule,
        RouterLink,
        MatDialogModule,
        MatFormFieldModule,
        MatInputModule,
        ReactiveFormsModule,
        GameModule,
        MatOptionModule,
        MatSelectModule
    ]
})
export class ManagerModule { }
