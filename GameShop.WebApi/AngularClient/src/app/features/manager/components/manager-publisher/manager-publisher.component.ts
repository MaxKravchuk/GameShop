import { Component, OnInit } from '@angular/core';
import { Publisher } from "../../../../core/models/Publisher";
import { PublisherService } from "../../../../core/services/publisherService/publisher.service";
import { MatDialog } from "@angular/material/dialog";
import { PagedList } from "../../../../core/models/PagedList";
import { PublisherCrudComponent } from "../dialogs/publisher-crud/publisher-crud.component";

@Component({
  selector: 'app-manager-publisher',
  templateUrl: './manager-publisher.component.html',
  styleUrls: ['./manager-publisher.component.scss']
})
export class ManagerPublisherComponent implements OnInit {

    publishers?: Publisher[] = [];

    pageSizePub!: number;

    totalCountPub!: number;

    pageIndexPub!: number;

    HasNextPub!: boolean;

    HasPreviousPub!: boolean;

    constructor(
        private publisherService: PublisherService,
        private dialog: MatDialog
    ) { }

    ngOnInit(): void {
        this.updatePublishers();
    }

    pageSizeChangePub(value: number): void {
        this.pageSizePub = value;
        this.pageIndexPub = 1;
        this.updatePublishers();
    }

    pageIndexChangePub(value: number): void {
        this.pageIndexPub = value;
        this.updatePublishers();
    }

    updatePublishers(): void {
        const pagedParams = {
            pageSize: this.pageSizePub,
            pageNumber: this.pageIndexPub
        };
        this.publisherService.getAllPublishersPaged(pagedParams).subscribe((pagedResult: PagedList<Publisher>):void => {
            this.publishers = pagedResult.Entities;
            this.totalCountPub = pagedResult.TotalCount;
            this.HasNextPub = pagedResult.HasNext;
            this.HasPreviousPub = pagedResult.HasPrevious;
        });
    }

    addPublisher(): void {
        const dialogRef = this.dialog.open(PublisherCrudComponent, {
            autoFocus: false,
            data: {
                publisher: null,
            }
        });

        dialogRef.afterClosed().subscribe((requireReload:boolean): void => {
            if(requireReload) {
                this.updatePublishers();
            }
        });
    }

    editDeletePublisher(publisher: Publisher): void {
        const dialogRef = this.dialog.open(PublisherCrudComponent, {
            autoFocus: false,
            data: {
                publisher: publisher,
            }
        });

        dialogRef.afterClosed().subscribe((requireReload:boolean): void => {
            if(requireReload) {
                this.updatePublishers();
            }
        });
    }
}
