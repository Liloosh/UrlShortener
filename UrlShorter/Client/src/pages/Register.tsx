import { useEffect, useState } from "react";
import { useRegisterMutation } from "../store/user/user.api";
import { useNavigate } from "react-router-dom";

const Register = () => {
    const [email, setEmail] = useState<string>("")
    const [password, setPassword] = useState<string>("")
    const [userName, setUserName] = useState<string>("")

    const navigate = useNavigate()
    const [register, result] = useRegisterMutation()

    const onRegister = () => {
        register(
            {
                email: email,
                password: password,
                userName: userName
            }
        )
    }

    useEffect(() => {
        if(result.isSuccess)
            navigate("/login")
    }, [result])

    return (
        <div className=" w-[40rem] bg-blue-950 text-white font-bold mt-4 mx-auto text-xl rounded-2xl p-10 flex flex-col gap-4">
            <div className="flex flex-col gap-2">
                <label>UserName</label>
                <input className="w-full h-10 rounded-lg border-2 border-black px-4 py-2 text-black" onChange={(e: React.ChangeEvent<HTMLInputElement>) => setUserName(e.target.value)}/>
            </div>
            <div className="flex flex-col gap-2">
                <label>Email</label>
                <input className="w-full h-10 rounded-lg border-2 border-black px-4 py-2 text-black" onChange={(e: React.ChangeEvent<HTMLInputElement>) => setEmail(e.target.value)} />
            </div>
            <div className="flex flex-col gap-2">
                <label>Password</label>
                <input className="w-full h-10 rounded-lg border-2 border-black px-4 py-2 text-black" onChange={(e: React.ChangeEvent<HTMLInputElement>) => setPassword(e.target.value)} />
            </div>
            <button onClick={() => onRegister()}>Register</button>
        </div>
    )
}

export default Register;