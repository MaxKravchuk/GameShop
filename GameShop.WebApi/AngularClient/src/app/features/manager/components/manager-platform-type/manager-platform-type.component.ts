import { Component, OnInit } from '@angular/core';
import { PlatformType } from "../../../../core/models/PlatformType";
import { PlatformTypeService } from "../../../../core/services/platformTypeService/platform-type.service";
import { MatDialog } from "@angular/material/dialog";
import { PagedList } from "../../../../core/models/PagedList";
import { PlatformTypeCrudComponent } from "../dialogs/platform-type-crud/platform-type-crud.component";

@Component({
  selector: 'app-manager-platform-type',
  templateUrl: './manager-platform-type.component.html',
  styleUrls: ['./manager-platform-type.component.scss']
})
export class ManagerPlatformTypeComponent implements OnInit {

    platformTypes?: PlatformType[] = [];

    pageSizePlt!: number;

    totalCountPlt!: number;

    pageIndexPlt!: number;

    HasNextPlt!: boolean;

    HasPreviousPlt!: boolean;

    constructor(
        private platformTypeService: PlatformTypeService,
        private dialog: MatDialog
    ) { }

    ngOnInit(): void {
        this.updatePlatformTypes();
    }

    pageSizeChangePlt(value: number): void {
        this.pageSizePlt = value;
        this.pageIndexPlt = 1;
        this.updatePlatformTypes();
    }

    pageIndexChangePlt(value: number): void {
        this.pageIndexPlt = value;
        this.updatePlatformTypes();
    }

    updatePlatformTypes(): void {
        const pagedParams = {
            pageSize: this.pageSizePlt,
            pageNumber: this.pageIndexPlt
        };
        this.platformTypeService.getAllPlatformTypesPaged(pagedParams).subscribe((pagedResult: PagedList<PlatformType>):void => {
            this.platformTypes = pagedResult.Entities;
            this.totalCountPlt = pagedResult.TotalCount;
            this.HasNextPlt = pagedResult.HasNext;
            this.HasPreviousPlt = pagedResult.HasPrevious;
        });
    }

    addPlatformType(): void {
        const dialogRef = this.dialog.open(PlatformTypeCrudComponent, {
            autoFocus: false,
            data: {
                platformType: null,
            }
        });

        dialogRef.afterClosed().subscribe((requireReload:boolean): void => {
            if(requireReload) {
                this.updatePlatformTypes();
            }
        });
    }

    editDeletePlatformType(platformType: PlatformType): void {
        const dialogRef = this.dialog.open(PlatformTypeCrudComponent, {
            autoFocus: false,
            data: {
                platformType: platformType,
            }
        });

        dialogRef.afterClosed().subscribe((requireReload:boolean): void => {
            if(requireReload) {
                this.updatePlatformTypes();
            }
        });
    }
}
