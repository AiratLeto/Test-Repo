import { BaseGetRequest, BaseGetResponse } from "../types/BaseRequest";
import { DictionaryItem } from "../types/Dictionary";
import { ApiConnection } from "./ApiConnection";

/** Сервис справочника брендов */
class BrandService {
    static get RootPrefix() { return '/brand' };

    static async GetListAsync(request: BaseGetRequest) {
        const response = await ApiConnection.get<BaseGetResponse<DictionaryItem>>(this.RootPrefix, { params: request });
        return response.data;
    }

    static async CreateAsync(request: DictionaryItem) {
        const response = await ApiConnection.post<number>(this.RootPrefix, request);
        return response.data;
    }

    static async UpdateAsync(id: string, request: DictionaryItem) {
        await ApiConnection.put(`${this.RootPrefix}/${id}`, request);
    }

    static async DeleteAsync(id: string) {
        await ApiConnection.delete(`${this.RootPrefix}/${id}`);
    }
}

export default BrandService;