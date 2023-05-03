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
    ) {
    }

    public openWithMessage(message: string): void {
        this.snackBar.open(`${message}`, 'Close', {
            duration: 4000,
            horizontalPosition: 'center',
            verticalPosition: 'top'
        });
    }

    public goBack(): void {
        this.location.back();
    }


}
