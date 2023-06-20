import { Directive, Input, TemplateRef, ViewContainerRef } from '@angular/core';
import { AuthService } from "../../../../core/services/authService/auth.service";

@Directive({
  selector: '[appIsInRole]'
})
export class IsInRoleDirective {

    constructor(
        private templateRef: TemplateRef<any>,
        private viewContainer: ViewContainerRef,
        private authService: AuthService
    ) { }

    @Input() set appIsInRole(role: string) {
        this.authService.getUserRole$().subscribe((userRole: string | null): void => {
            if(userRole === undefined || role !== userRole) {
                this.viewContainer.clear();
                return;
            }
            this.viewContainer.createEmbeddedView(this.templateRef);
        });
    }
}
