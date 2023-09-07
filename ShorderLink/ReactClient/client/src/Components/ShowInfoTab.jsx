import axios from "axios";
import { SERVER_URL } from "../Constants";
import { useParams } from 'react-router-dom';
import { getCookie } from "../Services/CookieService";

function ShowInfoTab(){

    const { value } = useParams();

    axios.get(SERVER_URL+'Links/Details/'+value,{
        headers: {
            'Authorization': `Bearer ${getCookie('token')}`
        },
    })
    .then(function (response) {
        document.getElementById('infoPage').innerHTML = response.data;
    })
    .catch(function (error) {
        console.error(error);
    });

    console.log(value);
    if(Object.keys(value).length === 0) return null;
    return(
        <>
            <div id="infoPage">
            </div>
        </>
    )
}

export default ShowInfoTab;