import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HTTP_INTERCEPTORS, HttpClientModule } from "@angular/common/http";
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LayoutModule } from "./layout/layout.module";
import { AppRoutingModule } from "./app-routing.module";
import { SharedService } from "./core/services/helpers/sharedService/shared.service";
import { AuthInterceptor } from "./core/interceptors/auth/auth.interceptor";
import { GameModule } from "./features/game/game.module";
import { NgOptimizedImage } from "@angular/common";
import { LoadingInterceptor } from "./core/interceptors/loading/loading.interceptor";
import { SharedModule } from "./features/shared/shared.module";

@NgModule({
    declarations: [
        AppComponent,
    ],
    imports: [
        BrowserModule,
        AppRoutingModule,
        HttpClientModule,
        BrowserAnimationsModule,
        LayoutModule,
        GameModule,
        NgOptimizedImage,
        SharedModule,
    ],
    providers: [
        SharedService,
        {
            provide: HTTP_INTERCEPTORS,
            useClass: AuthInterceptor,
            multi: true
        },
        {
            provide: HTTP_INTERCEPTORS,
            useClass: LoadingInterceptor,
            multi: true
        }
    ],
    bootstrap: [AppComponent]
})
export class AppModule {
}
