import { useState } from "react";
import { useCreateShortUrlMutation, useDeleteShortUrlMutation, useGetAllUrlsQuery } from "../store/url/url.api";
import { useAppSelector } from "../hooks/redux";
import { Link } from "react-router-dom";

const HomePage = () => {
    const {data} = useGetAllUrlsQuery()
    const [createUrl, result] = useCreateShortUrlMutation()
    const [deleteUrl, result1] = useDeleteShortUrlMutation()
    const user = useAppSelector(state => state.user)
    const [url, setUrl] = useState<string>("")

    const onCreate = () => {
        createUrl({
            fullUrl: url,
            userId: user.userId!,
            token: user.token!
        })
        setUrl("")
    }

    const onDelete = (event: React.MouseEvent<HTMLButtonElement, MouseEvent>, id: number) => {
        event.preventDefault()
        deleteUrl({
            id: parseInt(id!.toString()),
            token: user.token
        })
    }

    return (
        <>
            {user.authorized && <div className=" bg-blue-950 mt-4 w-[50rem] text-white flex justify-center items-center  py-10 font-bold text-xl mx-auto rounded-2xl">
                <label>Url</label>
                <input className=" border-2 w-[60%] mx-4 h-10 rounded-lg border-black px-4 py-2 text-black" value={url} onChange={(event: React.ChangeEvent<HTMLInputElement>) => setUrl(event.target.value)} />
                <button onClick={() => onCreate()}>Add</button>
            </div>}
            <div className="bg-blue-950 mt-4 w-[50rem] mx-auto rounded-xl flex flex-col p-4 text-white gap-6">
                {data?.map((item, index) => {
                    return <Link className=" " to={"view/" + item.id} key={index}>
                        <div className="w-full flex gap-4 font-bold text-lg">
                            <h1 className="w-[60%]">FullUrl</h1>
                            <h1>ShortUrl</h1>
                        </div>
                        <div className="w-full flex gap-4 items-center">
                            <h1 className=" min-w-[60%]">{item.fullUrl}</h1>
                            <h1 className="">{item.shortUrl}</h1>
                            {user.userId === item.userId && <button className="text-center w-full bg-red-600 rounded-xl h-7" onClick={(event: React.MouseEvent<HTMLButtonElement, MouseEvent>) => onDelete(event, item.id)}>Delete</button>}
                        </div>
                    </Link>
                })}
            </div>
        </>
    )
}

export default HomePage;