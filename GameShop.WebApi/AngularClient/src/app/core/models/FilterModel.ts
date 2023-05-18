export interface FilterModel {
    gameFiltersDTO: {
        genresId?: string[];
        platformsId?: string[];
        publishersId?: string[];
        dateOption?: string;
        gameName?: string;
        priceFrom?: string;
        priceTo?: string;
        sortingOption?: string;
        pageNumber?: number;
        pageSize?: number;
    };
}
