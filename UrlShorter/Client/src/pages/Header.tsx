import { Link } from "react-router-dom"
import { useAppDispatch, useAppSelector } from "../hooks/redux"
import { useCookies } from "react-cookie"
import { userSlice } from "../store/user/user.slice"

const Header = () => {
    const user = useAppSelector(state => state.user)
    const [cookies, setCookies, removeCookies] = useCookies(["token"])
    const dispatch = useAppDispatch()

    const onLogout = () => {
        removeCookies("token")
        dispatch(userSlice.actions.unauthorize())
    }

    return (

        <header className=" h-20 bg-blue-950 flex justify-end px-4"> 
            {!user.authorized && <div className=" w-40 flex justify-between h-full items-center text-white font-bold text-xl">
                <Link to={"login"} className="">Login</Link>
                <Link to={"register"}>Register</Link>
            </div>}
            {user.authorized && <div className=" min-w-40 flex justify-between h-full items-center text-white font-bold text-xl">
                <h1>{user.userName}</h1>
                <button onClick={() => onLogout()}>Logout</button>
            </div>}
        </header>
    )
}

export default Header