import { useNavigate, useParams } from "react-router-dom"
import { useDeleteShortUrlMutation, useGetUrlByIdQuery } from "../store/url/url.api"
import { useAppSelector } from "../hooks/redux"
import { useEffect } from "react"

type paramType = {
    id: string
}

const InfoPage = () => {
    const {id} = useParams<paramType>()
    const user = useAppSelector(state => state.user)
    const [deleteUrl, result] = useDeleteShortUrlMutation()
    const navigate = useNavigate();
    const {data} = useGetUrlByIdQuery({
        id: parseInt(id!),
        token: user.token
    })

    const onDelete = () => {
        deleteUrl({
            id: parseInt(id!),
            token: user.token
        })
    }

    useEffect(() => {
        if(result.isSuccess){
            navigate("/")
        }
    }, [result])

    return (
        <>
        <div className=" bg-blue-950 mt-4 mx-auto p-10 rounded-2xl w-[30rem] text-white flex flex-col gap-4">
            <h1 className="font-bold text-2xl">Full Information</h1>
            <div className="flex items-center gap-4">
                <h1 className="font-bold text-xl">Full Url:</h1>
                <h1 className="w-[30%]">{data?.url?.fullUrl}</h1>
            </div>
            <div className="flex items-center gap-4">
                <h1 className="font-bold text-xl">Short Url:</h1>
                <h1 className="w-[30%]">{data?.url?.shortUrl}</h1>
            </div>
            <div className="flex items-center gap-4">
                <h1 className="font-bold text-xl">Created Date:</h1>
                <h1>{data?.url?.createdDate.slice(0,10)}</h1>
            </div>
            <div className="flex items-center gap-4">
                <h1 className="font-bold text-xl">Created By:</h1>
                <h1>{data?.url?.user.userName}</h1>
            </div>
            {user.authorized && data?.url?.userId === user.userId  && <button className=" bg-red-600 rounded-xl h-10" onClick={() => onDelete()}>Delete</button>}
        </div>
        </>
    )
}

export default InfoPage