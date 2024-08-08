import { LoginResponseEnum } from "../Enums/LoginResponseEnum";

export interface LoginResponse{
    response: LoginResponseEnum,
    token: string | null
}