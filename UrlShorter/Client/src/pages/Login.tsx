import { useEffect, useState } from "react"
import { useLoginMutation } from "../store/user/user.api"
import { useAppDispatch } from "../hooks/redux"
import { userSlice } from "../store/user/user.slice"
import { useNavigate } from "react-router-dom"
import { useCookies } from "react-cookie"

const Login = () => {

    const [email, setEmail] = useState<string>("")
    const [password, setPassword] = useState<string>("")
    const [cookies, setCookies] = useCookies(["token"])
    const dispatch = useAppDispatch()
    const navigate = useNavigate()
    const [login, result] = useLoginMutation()

    const onLogin = () => {
        login({
            email: email,
            password: password
        })
    }

    useEffect(() => {
        if(result.data !== undefined && result.data !== null){
            dispatch(userSlice.actions.initUser(result.data.token!))
            dispatch(userSlice.actions.authorize())
            setCookies("token", result.data.token!)
            navigate("/")
        }
    }, [result])

    return (
        <div className=" w-[40rem] bg-blue-950 text-white font-bold mt-4 mx-auto text-xl rounded-2xl p-10 flex flex-col gap-4">
            
                <div className="flex flex-col gap-2">
                    <label>Email</label>
                    <input className="w-full h-10 rounded-lg border-2 border-black px-4 py-2 text-black" onChange={(e: React.ChangeEvent<HTMLInputElement>) => setEmail(e.target.value)}/>
                </div>
                <div className="flex flex-col gap-2">
                    <label >Password</label>
                    <input className="w-full h-10 rounded-lg border-2 border-black px-4 py-2 text-black" onChange={(e: React.ChangeEvent<HTMLInputElement>) => setPassword(e.target.value)} />
                </div>
                <button onClick={() => onLogin()}>Login</button>

        </div>
    )
}

export default Login