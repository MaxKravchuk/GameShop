import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from "./header/header.component";
import { FooterComponent } from "./footer/footer.component";
import { MainComponent } from "./main/main.component";
import { RouterModule } from "@angular/router";
import { MatToolbarModule } from "@angular/material/toolbar";
import { MatButtonModule } from "@angular/material/button";
import { GameModule } from "../features/game/game.module";
import { HasPermissionsDirective } from "../core/directives/has-permision/has-permissions.directive";
import { AppModule } from "../app.module";
import { IsAuthorizedDirective } from "../core/directives/is-authorized/is-authorized.directive";


@NgModule({
    declarations: [
        HeaderComponent,
        FooterComponent,
        MainComponent,
        HasPermissionsDirective,
        IsAuthorizedDirective
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
    ]
})
export class LayoutModule {
}
