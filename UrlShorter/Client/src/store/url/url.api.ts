import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";
import { Url } from "../../models/url/Url";
import { UrlGetById } from "../../models/url/UrlGetById";
import { UrlGetByIdResponse } from "../../models/url/UrlGetByIdResponse";
import { UrlDto } from "../../models/url/UrlDto";
import { UrlGetByShortUrl } from "../../models/url/UrlGetByShortUrl";

export const urlApi = createApi({
    reducerPath: "url/api",
    baseQuery: fetchBaseQuery({baseUrl: "https://localhost:7127/api/Url"}),
    tagTypes: ['Url'],
    endpoints: (build) => ({
        getAllUrls: build.query<Url[], void>({
            query: () => ({
                url: ""
            }),
            providesTags: ['Url'],
        }),
        getUrlById: build.query<UrlGetByIdResponse, UrlGetById>({
            query: (url: UrlGetById) => ({
                url: url.id.toString(),
                headers: {"Content-Type": "application/json", "Authorization": "Bearer " + url.token},
                
            }),
        }),
        getUrlByShortUrl: build.query<UrlGetByIdResponse, UrlGetByShortUrl>({
            query: (url: UrlGetByShortUrl) => ({
                url: url.shortUrl,
                headers: {"Content-Type": "application/json", "Authorization": "Bearer " + url.token},
                
            }),
        }),
        deleteShortUrl: build.mutation<UrlGetByIdResponse, UrlGetById>({
            query: (url: UrlGetById) => ({
                url: url.id.toString(),
                headers: {"Content-Type": "application/json", "Authorization": "Bearer " + url.token},
                method: "DELETE"
            }),
            invalidatesTags: ["Url"]
        }),
        createShortUrl: build.mutation<UrlGetByIdResponse, UrlDto>({
            query: (url: UrlDto) => ({
                url: "",
                headers: {"Content-Type": "application/json", "Authorization": "Bearer " + url.token},
                method: "POST",
                body: {
                    fullUrl: url.fullUrl,
                    userId: url.userId
                }
            }),
            invalidatesTags: ["Url"]
        }),
    })
})

export const {useGetAllUrlsQuery, useGetUrlByIdQuery, useCreateShortUrlMutation, useDeleteShortUrlMutation, useLazyGetUrlByShortUrlQuery} = urlApi