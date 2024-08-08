import { RegisterResponseEnum } from "../Enums/RegisterResponseEnum";

export interface RegisterResponse {
    response: RegisterResponseEnum,
    message: string[]
}