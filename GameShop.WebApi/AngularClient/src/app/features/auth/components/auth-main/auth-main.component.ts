import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { UtilsService } from "../../../../core/services/helpers/utilsService/utils-service";
import { LoginModel } from "../../../../core/models/AuthModels/LoginModel";
import { AuthService } from "../../../../core/services/authService/auth.service";
import { ActivatedRoute, Data, Router } from "@angular/router";
import { RegistrationModel } from "../../../../core/models/AuthModels/RegistrationModel";

@Component({
  selector: 'app-auth-main',
  templateUrl: './auth-main.component.html',
  styleUrls: ['./auth-main.component.scss']
})
export class AuthMainComponent implements OnInit {

    form!: FormGroup;

    isLoginSucceeded: boolean = false;

    action?: string;

    constructor(
        private formBuilder: FormBuilder,
        private utilsService: UtilsService,
        private authService: AuthService,
        private router: Router,
        private route: ActivatedRoute) { }

    ngOnInit(): void {
        this.route.data.subscribe((data: any): void => {
            this.action = data.action;
        });

        this.form = this.formBuilder.group({
            NickName: ['', [Validators.required, Validators.minLength(1)]],
            Password: ['', [Validators.required, Validators.minLength(1)]],
            Email: [{value: '', disabled: true}],
        });

        if (this.action === 'register') {
            this.form.controls['Email'].enable();
            this.form.controls['Email'].setValidators([Validators.required, Validators.email]);
        }
    }

    onNoClick(): void {
        this.utilsService.goBack();
    }

    onLogIn(): void {
        if (!this.form.valid) {
            this.utilsService.openWithMessage('Please fill all the fields');
        }

        let data: LoginModel = { userCreateDTO: {} };
        data.userCreateDTO.nickName = this.form.value.NickName;
        data.userCreateDTO.password = this.form.value.Password;

        this.authService.login(data.userCreateDTO)
            .subscribe({
                next: (): void => {
                    this.handleLoginSuccess();
                }
            });
    }

    onRegister(): void {
        if (!this.form.valid) {
            this.utilsService.openWithMessage('Please fill all the fields');
        }

        const data: RegistrationModel = this.form.value as RegistrationModel;
        this.authService.register(data)
            .subscribe({
                next: (): void => {
                    setTimeout((): void => {
                        this.utilsService.openWithMessage('Registration successful');
                    },500);
                    this.router.navigate(['/']);
                }
            })
    }

    private handleLoginSuccess(): void {
        this.isLoginSucceeded = true;
        this.router.navigate(['/']);
    }
}
