import { BaseGetRequest, BaseGetResponse } from "../types/BaseRequest";
import { GetCarRespone, SaveCarRequest } from "../types/CarRequest";
import { ApiConnection } from "./ApiConnection";

/** Сервис автомобилей */
class CarService {
    static get RootPrefix() { return '/Car' };

    static async GetListAsync(request: BaseGetRequest) {
        const response = await ApiConnection.get<BaseGetResponse<GetCarRespone>>(this.RootPrefix, { params: request });
        return response.data;
    }

    static async CreateAsync(request: SaveCarRequest) {
        const response = await ApiConnection.post<number>(this.RootPrefix, request);
        return response.data;
    }

    static async UpdateAsync(id: string, request: SaveCarRequest) {
        await ApiConnection.put(`${this.RootPrefix}/${id}`, request);
    }

    static async DeleteAsync(id: string) {
        await ApiConnection.delete(`${this.RootPrefix}/${id}`);
    }
}

export default CarService;