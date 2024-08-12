interface CarRequest {
    id?: string,
    brandId: string,
    bodyTypeId: string,
    name: string,
    seatsCount: number,
    url?: string,
}

/** ДТО для сохранения */
export interface SaveCarRequest extends CarRequest { }

/** ДТО результата получения */
export interface GetCarRespone extends CarRequest {
    brandName: string,
    bodyTypeName: string,
    createdOn?: string,
}