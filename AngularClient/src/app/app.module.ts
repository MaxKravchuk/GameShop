import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import {HttpClientModule} from "@angular/common/http";

import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {RouterModule, Routes} from "@angular/router";
import { MainComponent } from './layout/main/main.component';
import {LayoutModule} from "./layout/layout.module";
import {AppRoutingModule} from "../app-routing.module";
import { GameCommentComponent } from './features/comment/components/game-comment/game-comment.component';

const appRoutes: Routes = [
  {
    path: '',
    component: MainComponent
  }
  ];

@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    RouterModule.forRoot(appRoutes),
    LayoutModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
