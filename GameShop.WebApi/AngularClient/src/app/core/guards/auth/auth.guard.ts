import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from "../../services/authService/auth.service";

@Injectable({
  providedIn: 'root'
})
export class AuthGuard {

    constructor(
        private router: Router,
        private authService: AuthService
    ) { }

    canActivate(): boolean {
        let isAuthorized: boolean = this.authService.isAuthorized();

        if (!isAuthorized) {
            this.router.navigate(['auth/login']);
        }

        return isAuthorized;
    }

}
