import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Router, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from "../../services/authService/auth.service";

@Injectable({
  providedIn: 'root'
})
export class RoleGuard {

    constructor(
        private router: Router,
        private authService: AuthService)
    { }

    canActivate(
        route: ActivatedRouteSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {

        const allowedRoles = route.data['allowedRoles'];
        const actualRole = this.authService.getRole();
        if (!allowedRoles.includes(actualRole)) {
            this.router.navigate(['auth/login']);
            return false;
        }
        return true;
    }

}
