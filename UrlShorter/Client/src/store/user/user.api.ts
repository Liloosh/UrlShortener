import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";
import { RegisterResponse } from "../../models/RegisterResponse";
import { RegisterRequest } from "../../models/RegisterRequest";
import { LoginResponse } from "../../models/LoginResponse";
import { LoginRequest } from "../../models/LoginRequest";
import { RefreshTokenResponse } from "../../models/RefreshTokenResponse";

export const userApi = createApi({
    reducerPath: "user/api",
    baseQuery: fetchBaseQuery({baseUrl: "https://localhost:7127/api/User"}),
    endpoints: (build) => ({
        register: build.mutation<RegisterResponse, RegisterRequest>({
            query: (user: RegisterRequest) => ({
                method: "POST",
                url: "register",
                headers: {"Content-Type": "application/json"},
                body: user
            })
        }),
        login: build.mutation<LoginResponse, LoginRequest>({
            query: (user: LoginRequest) => ({
                url: "login",
                method: "POST",
                headers: {"Content-Type": "application/json"},
                body: user
            })
        }),
        refreshToken: build.mutation<RefreshTokenResponse, void>({
            query: () => ({
                url: "refresh-token",
                method: "POST",
                headers: {"Content-Type": "application/json"},
            })
        })
    })
})

export const {useLoginMutation, useRegisterMutation, useRefreshTokenMutation} = userApi