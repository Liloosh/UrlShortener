import {PayloadAction, createSlice } from "@reduxjs/toolkit";
import {JwtPayload, jwtDecode} from 'jwt-decode';

interface User{
    userId: string,
    userName: string,
    userEmail: string,
    token:string,
    role: string,
    authorized: boolean
}

interface CustomJwtPayload extends JwtPayload {
    UserId?: string,
    UserName?: string,
    Email?: string
    "http://schemas.microsoft.com/ws/2008/06/identity/claims/role": string
  }

const initialState: User = {
    userId: "",
    userName: "",
    userEmail: "",
    token: "",
    role: "",
    authorized: false
}

export const userSlice = createSlice({
    name: "user",
    initialState,
    reducers: {
        initUser(state, action: PayloadAction<string>){
            let decodedToken: CustomJwtPayload = jwtDecode<CustomJwtPayload>(action.payload)
            state.userId = decodedToken.UserId!
            state.userEmail = decodedToken.Email!
            state.userName = decodedToken.UserName!
            state.role = decodedToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"]!
            state.token = action.payload
        },
        authorize(state){
            state.authorized = true
        },
        unauthorize(state){
            state.authorized = false
        }
    }
})

export const userActions = userSlice.actions
export const userReducer = userSlice.reducer