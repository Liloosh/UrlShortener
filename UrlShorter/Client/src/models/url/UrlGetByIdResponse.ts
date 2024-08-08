import { UrlEnum } from "../../Enums/UrlEnum";
import { Url, UrlResponse } from "./Url";

export interface UrlGetByIdResponse{
    response: UrlEnum,
    message: string | null,
    url: UrlResponse | null
}