import { UserResponse } from "../UserResponse";

export interface Url{
    id: number,
    fullUrl: string,
    shortUrl: string,
    userId: string,
    createdDate: string,
}

export interface UrlResponse {
    id: number,
    fullUrl: string,
    shortUrl: string,
    userId: string,
    createdDate: string,
    user: UserResponse
}