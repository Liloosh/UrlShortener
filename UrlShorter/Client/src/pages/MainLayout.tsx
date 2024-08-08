import { Outlet } from "react-router-dom"
import Header from "./Header"
import { useEffect } from "react"
import { useAppDispatch, useAppSelector } from "../hooks/redux"
import { useCookies } from "react-cookie"
import { userSlice } from "../store/user/user.slice"

const MainLayout = () => {
    const user = useAppSelector(state => state.user)
    const [cookies, setCookies] = useCookies(["token"])
    const dispatch = useAppDispatch()
    useEffect(() => {
        if(!user.authorized){
            if(cookies.token !== null && cookies.token !== undefined){
                dispatch(userSlice.actions.initUser(cookies.token))
                dispatch(userSlice.actions.authorize())
            }
        }
    }, [])

    return <>
        <Header/>
        <Outlet/>
    </>
}

export default MainLayout