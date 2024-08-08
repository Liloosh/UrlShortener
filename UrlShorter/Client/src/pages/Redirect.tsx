import { useEffect } from "react";
import { useLazyGetUrlByShortUrlQuery } from "../store/url/url.api";
import { useParams } from "react-router-dom";
import { useAppSelector } from "../hooks/redux";

type Params = {
    shortUrl:string
}

const Redirect = () => {
    const user = useAppSelector(state => state.user);
    const {shortUrl} = useParams<Params>()
    const [getShortUrl, result] = useLazyGetUrlByShortUrlQuery()
    useEffect(() => {
        getShortUrl({
            shortUrl: shortUrl!,
            token: user.token
        })
    }, []);

    useEffect(() => {
        if(result.data !== null && result.data !== undefined){
            window.location.href = result.data.url!.fullUrl;
        }
    }, [result])

    return (
        <div>
          <p>Redirecting...</p>
        </div>
    );
}

export default Redirect