import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from "../../services/authService/auth.service";

@Injectable({
  providedIn: 'root'
})
export class UnAuthGuard {
    constructor(
        private router: Router,
        private authService: AuthService
    ) { }

    canActivate(): boolean {
        let isAuthorized: boolean = this.authService.isAuthorized();
        return !isAuthorized;
    }

}
