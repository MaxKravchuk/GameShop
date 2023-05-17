import { Component, OnInit } from '@angular/core';
import { Genre } from "../../../../core/models/Genre";
import { PlatformType } from "../../../../core/models/PlatformType";
import { Publisher } from "../../../../core/models/Publisher";
import { GenreService } from "../../../../core/services/genreService/genre.service";
import { PlatformTypeService } from "../../../../core/services/platformTypeService/platform-type.service";
import { PublisherService } from "../../../../core/services/publisherService/publisher.service";
import { FormBuilder, FormGroup } from "@angular/forms";
import { SharedService } from "../../../../core/services/helpers/sharedService/shared.service";
import { FilterShared } from "../../../../core/models/helpers/FilterShared";

@Component({
  selector: 'app-game-filters',
  templateUrl: './game-filters.component.html',
  styleUrls: ['./game-filters.component.css']
})
export class GameFiltersComponent implements OnInit {

    genres: Genre[] = [];

    platforms: PlatformType[] = [];

    publishers: Publisher[] = [];

    form!: FormGroup;

    constructor(
        private formBuilder: FormBuilder,
        private genreService: GenreService,
        private platformTypeService: PlatformTypeService,
        private publisherService: PublisherService,
        private sharedService: SharedService<FilterShared>
    ) { }

    ngOnInit(): void {
        this.form = this.formBuilder.group({
            SortedBy: [''],
            GameName: [''],
            PriceFrom: [''],
            PriceTo: [''],
            DateFilter: [''],
        });
        this.genreService.getAllGenres().subscribe(
            (genres: Genre[]) => this.genres = genres
        );

        this.platformTypeService.getAllPlatformTypes().subscribe(
            (platforms: PlatformType[]) => this.platforms = platforms
        );

        this.publisherService.getAllPublishers().subscribe(
            (publishers: Publisher[]) => this.publishers = publishers
        );
    }

    setFilters(): void {
        let filterValues = this.form.value;
        const model: FilterShared = { FilterString: this.buildFilterString(filterValues) };
        this.sharedService.sendData(model);
    }

    private buildFilterString(filterValues: any): string {
        const dateFilterString: string = `?gameFiltersDTO.dateOption=${filterValues.DateFilter}`;
        const gameNameFilterString: string = `gameFiltersDTO.gameName=${filterValues.GameName}`;
        const priceFromFilterString: string = `gameFiltersDTO.priceFrom=${filterValues.PriceFrom}`;
        const priceToFilterString: string = `gameFiltersDTO.priceTo=${filterValues.PriceTo}`;
        const sortingOptionFilterString: string = `gameFiltersDTO.sortingOption=${filterValues.SortedBy}`;
        let filterString: string = dateFilterString + '&'
            + gameNameFilterString + '&'
            + priceFromFilterString + '&'
            + priceToFilterString + '&'
            + sortingOptionFilterString+ '&';
        return filterString;
    }
}
