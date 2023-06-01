import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GenreRoutingModule } from "./genre-routing.module";
import { MatButtonModule } from "@angular/material/button";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { GenreCreateComponent } from './components/genre-create/genre-create.component';


@NgModule({
    declarations: [

    
    GenreCreateComponent
  ],
    imports: [
        CommonModule,
        GenreRoutingModule,
        MatButtonModule,
        FormsModule,
        MatFormFieldModule,
        MatInputModule,
        ReactiveFormsModule
    ]
})
export class GenreModule {
}
