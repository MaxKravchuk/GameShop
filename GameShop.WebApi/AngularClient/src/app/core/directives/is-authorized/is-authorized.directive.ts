import { Directive, ElementRef, OnInit, Renderer2 } from '@angular/core';
import { AuthService } from "../../services/authService/auth.service";

@Directive({
  selector: '[appIsAuthorized]'
})
export class IsAuthorizedDirective implements OnInit {

    constructor(
        private elementRef: ElementRef,
        private renderer: Renderer2,
        private authService: AuthService
    ) { }

    ngOnInit(): void {
        const isAuthorized: boolean = this.authService.isAuthorized();

        if (isAuthorized) {
            this.renderer.removeAttribute(this.elementRef.nativeElement, 'disabled');
        }
        else {
            this.renderer.setAttribute(this.elementRef.nativeElement, 'disabled', 'true');
        }
    }
}
