import { Injectable } from '@angular/core';
import { AuthService } from "../../services/authService/auth.service";

@Injectable({
  providedIn: 'root'
})
export class LoginGuard {
    constructor(private authService: AuthService) { }

    canActivate(): boolean {
        let isAuthorized: boolean = this.authService.isAuthorized();
        return !isAuthorized;
    }
}
