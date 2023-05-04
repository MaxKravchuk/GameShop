import { Component, OnInit } from '@angular/core';
import { PublisherService } from "../../../../core/services/publisherService/publisher.service";
import { Publisher } from "../../../../core/models/Publisher";
import { ActivatedRoute } from "@angular/router";
import { Game } from "../../../../core/models/Game";
import { UtilsService } from "../../../../core/services/helpers/utilsService/utils-service";

@Component({
    selector: 'app-publisher-main',
    templateUrl: './publisher-main.component.html',
    styleUrls: ['./publisher-main.component.css']
})
export class PublisherMainComponent implements OnInit {

    publisher?: Publisher;
    companyName?: string | null;
    games?: Game[] = [];

    constructor(
        private publisherService: PublisherService,
        private activeRoute: ActivatedRoute,
        private utilsService: UtilsService
    ) {}

    ngOnInit(): void {
        this.companyName = this.activeRoute.snapshot.paramMap.get('CompanyName');
        if (this.companyName != null) {
            this.getPublisherByCompanyName();
        } else {
            this.utilsService.goBack();
        }
    }

    getPublisherByCompanyName() {
        this.publisherService.getPublisherByCompanyName(this.companyName!).subscribe(
            (data: Publisher) => {
                this.publisher = data;
                this.games = data.GameReadListDTOs;
            }
        );
    }

}
