import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from "@angular/forms";

@Component({
  selector: 'app-auth-main',
  templateUrl: './auth-main.component.html',
  styleUrls: ['./auth-main.component.css']
})
export class AuthMainComponent implements OnInit {

    form!: FormGroup;

    constructor(private formBuilder: FormBuilder) { }

    ngOnInit(): void {
        this.form = this.formBuilder.group({
            Username: ['', Validators.minLength(1)],
            Password: ['', Validators.minLength(1)]
        });
    }

}
