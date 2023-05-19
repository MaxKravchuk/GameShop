export interface FilterModel {

    gameFiltersDTO: {
        genreIds?: string[];

        platformTypeIds?: string[];

        publisherIds?: string[];

        dateOption?: string;

        gameName?: string;

        priceFrom?: string;

        priceTo?: string;

        sortingOption?: string;

        pageNumber?: number;

        pageSize?: number;
    };
}
