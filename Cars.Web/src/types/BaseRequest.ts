/** Базовая пагинация для GET запроса списка */
export interface BaseGetRequest {
    pageSize?: number,
    pageNumber?: number,
    orderBy?: number,
    isAscending?: boolean,
}

/** Базовый ответ на GET запрос списка */
export interface BaseGetResponse<T> {
    entities: T[],
    totalCount: number,
}