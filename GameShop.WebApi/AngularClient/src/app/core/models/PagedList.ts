export interface PagedList<T> {

    Entities?: T[];

    HasNext: boolean;

    HasPrevious: boolean;

    TotalCount: number;
}
