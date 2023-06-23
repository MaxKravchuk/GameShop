import { NgModule } from '@angular/core';
import { CommonModule, NgOptimizedImage } from '@angular/common';
import { HeaderComponent } from "./header/header.component";
import { FooterComponent } from "./footer/footer.component";
import { MainComponent } from "./main/main.component";
import { RouterModule } from "@angular/router";
import { MatToolbarModule } from "@angular/material/toolbar";
import { MatButtonModule } from "@angular/material/button";
import { GameModule } from "../features/game/game.module";
import { SharedModule } from "../features/shared/shared.module";
import { MenuComponent } from './menu/menu.component';
import { MatIconModule } from "@angular/material/icon";
import { NewsSectionComponent } from './news-section/news-section.component';


@NgModule({
    declarations: [
        HeaderComponent,
        FooterComponent,
        MainComponent,
        MenuComponent,
        NewsSectionComponent,
    ],
    exports: [
        HeaderComponent,
        FooterComponent,
        MainComponent,
        MenuComponent,
        NewsSectionComponent
    ],
    imports: [
        CommonModule,
        RouterModule,
        MatToolbarModule,
        MatButtonModule,
        GameModule,
        SharedModule,
        MatIconModule,
        NgOptimizedImage,
    ]
})
export class LayoutModule {
}
