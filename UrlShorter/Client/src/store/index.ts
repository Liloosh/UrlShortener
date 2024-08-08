import { combineReducers, configureStore } from "@reduxjs/toolkit";
import { userApi } from "./user/user.api";
import { urlApi } from "./url/url.api";
import { userReducer } from "./user/user.slice";

const reducers = combineReducers({
    [userApi.reducerPath]: userApi.reducer,
    [urlApi.reducerPath]: urlApi.reducer,
    user: userReducer
})

export const store = configureStore({
    reducer: reducers,
    middleware: (getDefaultMiddleware) => getDefaultMiddleware().concat(userApi.middleware).concat(urlApi.middleware)
})

export type RootState = ReturnType<typeof store.getState>
export type AppDispatch = typeof store.dispatch