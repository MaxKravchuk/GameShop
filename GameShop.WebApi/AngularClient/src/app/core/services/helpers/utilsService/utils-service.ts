import { Injectable } from '@angular/core';
import { Location } from "@angular/common";
import { MatSnackBar } from "@angular/material/snack-bar";

@Injectable({
    providedIn: 'root'
})
export class UtilsService {

    constructor(
        private snackBar: MatSnackBar,
        private location: Location
    ) {}

    openWithMessage(message: string): void {
        this.snackBar.open(`${message}`, 'Close', {
            duration: 4000,
            horizontalPosition: 'center',
            verticalPosition: 'top'
        });
    }

    handleError(error: any): void{
        if (error.status === 400) {
            this.openWithMessage(error.statusText);
        }
        else if (error.status === 403) {
            this.openWithMessage(error.statusText);
        }
        else if (error.status === 404) {
            this.openWithMessage(error.statusText);
        }
        else if (error.status === 500) {
            this.openWithMessage('An internal server error occurred. Please try again later.');
        }
        else {
            this.openWithMessage('An unexpected error occurred. Please try again later.');
        }
    }

    goBack(): void {
        this.location.back();
    }
}
