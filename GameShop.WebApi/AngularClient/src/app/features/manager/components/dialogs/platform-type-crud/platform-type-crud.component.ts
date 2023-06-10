import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { PlatformType } from "../../../../../core/models/PlatformType";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { ManagerMainComponent } from "../../manager-main/manager-main.component";
import { PlatformTypeService } from "../../../../../core/services/platformTypeService/platform-type.service";
import { UtilsService } from "../../../../../core/services/helpers/utilsService/utils-service";

@Component({
  selector: 'app-platform-type-crud',
  templateUrl: './platform-type-crud.component.html',
  styleUrls: ['./platform-type-crud.component.css']
})
export class PlatformTypeCrudComponent implements OnInit {

    form!: FormGroup;

    platformType!: PlatformType;

    isAdding: boolean = false

    constructor(
        private formBuilder: FormBuilder,
        @Inject(MAT_DIALOG_DATA) private data : {platformType: PlatformType},
        private dialogRef: MatDialogRef<ManagerMainComponent>,
        private platformTypeService: PlatformTypeService,
        private utilsService: UtilsService
    ) { }

    ngOnInit(): void {
        this.form = this.formBuilder.group({
            Type: ['', Validators.minLength(1)],
        });

        if (this.data.platformType == null) {
            this.isAdding = true;
        }
        else {
            this.platformType = this.data.platformType;
        }
    }

    onNoClick(): void {
        this.dialogRef.close(false);
    }

    onSaveClick(): void {
        const newPlatformType: PlatformType = this.form.value as PlatformType;

        this.platformTypeService.createPlatformType(newPlatformType)
            .subscribe({
                next: (): void => {
                    this.utilsService.openWithMessage("Platform type created successfully!");
                    this.dialogRef.close(true);
                }
        });
    }

    onEditClick(): void {
        const newPlatformType: PlatformType = this.form.value as PlatformType;
        newPlatformType.Id = this.platformType.Id;
        this.platformTypeService.updatePlatformType(newPlatformType)
            .subscribe({
                next: (): void => {
                    this.utilsService.openWithMessage("Platform type updated successfully!");
                    this.dialogRef.close(true);
                }
        });
    }

    onDeleteClick(): void {
        this.platformTypeService.deletePlatformType(this.platformType.Id!)
            .subscribe({
                next: (): void => {
                    this.utilsService.openWithMessage("Platform type deleted successfully!");
                    this.dialogRef.close(true);
                }
        });
    }
}
