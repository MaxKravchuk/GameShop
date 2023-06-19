import { Directive, ElementRef, Input, OnInit, Renderer2 } from '@angular/core';
import { AuthService } from "../../services/authService/auth.service";

@Directive({
  selector: '[appHasPermissions]'
})
export class HasPermissionsDirective implements OnInit {

    @Input('appHasPermissions') allowedRole!: string;

    constructor(
        private elementRef: ElementRef,
        private renderer: Renderer2,
        private authService: AuthService
    ) { }

    ngOnInit(): void {
        let isInRole: boolean = this.authService.isInRole(this.allowedRole);

        if (isInRole) {
            this.renderer.removeAttribute(this.elementRef.nativeElement, 'disabled');
        }
        else {
            this.renderer.setAttribute(this.elementRef.nativeElement, 'disabled', 'true');
        }
    }
}
