import { Component, OnDestroy, OnInit } from '@angular/core';
import { Genre } from "../../../../core/models/Genre";
import { PlatformType } from "../../../../core/models/PlatformType";
import { Publisher } from "../../../../core/models/Publisher";
import { GenreService } from "../../../../core/services/genreService/genre.service";
import { PlatformTypeService } from "../../../../core/services/platformTypeService/platform-type.service";
import { PublisherService } from "../../../../core/services/publisherService/publisher.service";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { SharedService } from "../../../../core/services/helpers/sharedService/shared.service";
import { FilterModel } from "../../../../core/models/FilterModel";
import { forkJoin } from "rxjs";

@Component({
  selector: 'app-game-filters',
  templateUrl: './game-filters.component.html',
  styleUrls: ['./game-filters.component.scss']
})
export class GameFiltersComponent implements OnInit, OnDestroy {

    genres: Genre[] = [];

    platforms: PlatformType[] = [];

    publishers: Publisher[] = [];

    form!: FormGroup;

    isChanged: boolean = true;

    constructor(
        private formBuilder: FormBuilder,
        private genreService: GenreService,
        private platformTypeService: PlatformTypeService,
        private publisherService: PublisherService,
        private sharedService: SharedService<FilterModel>,
    ) { }

    ngOnInit(): void {

        document.getElementsByClassName('side-menu')[0].setAttribute('style', 'display: none;');

        this.form = this.formBuilder.group({
            SortedBy: [''],
            GameName: ['', Validators.minLength(3)],
            PriceFrom: [''],
            PriceTo: [''],
            DateFilter: [''],
            GenreIds: [''],
            PlatformIds: [''],
            PublisherIds: [''],
        });

        forkJoin([
            this.genreService.getAllGenres(),
            this.platformTypeService.getAllPlatformTypes(),
            this.publisherService.getAllPublishers()
        ]).subscribe(([genres, platformTypes, publishers]) => {
            this.genres = genres;
            this.platforms = platformTypes;
            this.publishers = publishers;
        });
    }

    ngOnDestroy(): void {
        document.getElementsByClassName('side-menu')[0].removeAttribute('style');
    }

    setFilters(): void {
        if (this.isChanged) {
            const filterValues = this.form.value;
            const queryParams: FilterModel = this.filterRequestToQueryParams(filterValues);
            this.sharedService.sendData(queryParams);
        }
        this.isChanged = false;
    }

    clearFilters(): void {
        this.form.reset({
            SortedBy: '',
            GameName: '',
            PriceFrom: '',
            PriceTo: '',
            DateFilter: '',
            GenreIds: '',
            PlatformIds: '',
            PublisherIds: '',
        });
        this.isChanged = true;
        this.setFilters();
    }

    handleChanges(): void {
        if(!this.isChanged) {
            this.isChanged = true;
        }
    }

    private filterRequestToQueryParams(filterRequest: any): FilterModel {

        let filterModel: FilterModel = { gameFiltersDTO: {} };

        if (filterRequest.GenreIds.length > 0) {
            filterModel.gameFiltersDTO.genreIds = filterRequest.GenreIds
        }

        if (filterRequest.PlatformIds.length > 0) {
            filterModel.gameFiltersDTO.platformTypeIds = filterRequest.PlatformIds;
        }

        if (filterRequest.PublisherIds.length > 0) {
            filterModel.gameFiltersDTO.publisherIds = filterRequest.PublisherIds;
        }

        filterModel.gameFiltersDTO.dateOption = filterRequest.DateFilter;
        filterModel.gameFiltersDTO.gameName = filterRequest.GameName;
        filterModel.gameFiltersDTO.priceFrom = filterRequest.PriceFrom;
        filterModel.gameFiltersDTO.priceTo = filterRequest.PriceTo;
        filterModel.gameFiltersDTO.sortingOption = filterRequest.SortedBy;

        return filterModel;
    }
}
