import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from "./header/header.component";
import { FooterComponent } from "./footer/footer.component";
import { MainComponent } from "./main/main.component";
import { RouterModule } from "@angular/router";
import { MatToolbarModule } from "@angular/material/toolbar";
import { MatButtonModule } from "@angular/material/button";
import { GameModule } from "../features/game/game.module";
import { SharedModule } from "../features/shared/shared.module";


@NgModule({
    declarations: [
        HeaderComponent,
        FooterComponent,
        MainComponent,
    ],
    exports: [
        HeaderComponent,
        FooterComponent,
        MainComponent
    ],
    imports: [
        CommonModule,
        RouterModule,
        MatToolbarModule,
        MatButtonModule,
        GameModule,
        SharedModule,
    ]
})
export class LayoutModule {
}
