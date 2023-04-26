import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PublisherMainComponent } from './components/publisher-main/publisher-main.component';
import {PublisherCreateComponent} from "./components/publisher-create/publisher-create.component";
import {PublisherRoutingModule} from "./publisher-routing-module";
import {MatFormFieldModule} from "@angular/material/form-field";
import {MatInputModule} from "@angular/material/input";
import {ReactiveFormsModule} from "@angular/forms";
import {MatButtonModule} from "@angular/material/button";
@NgModule({
  declarations: [
    PublisherMainComponent,
    PublisherCreateComponent
  ],
  imports: [
    CommonModule,
    PublisherRoutingModule,
    MatFormFieldModule,
    MatInputModule,
    ReactiveFormsModule,
    MatButtonModule
  ]
})
export class PublisherModule { }
