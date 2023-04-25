import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PublisherMainComponent } from './components/publisher-main/publisher-main.component';
import {PublisherCreateComponent} from "./components/publisher-create/publisher-create.component";
import {PublisherRoutingModule} from "./publisher-routing-module";
@NgModule({
  declarations: [
    PublisherMainComponent,
    PublisherCreateComponent
  ],
  imports: [
    CommonModule,
    PublisherRoutingModule
  ]
})
export class PublisherModule { }
