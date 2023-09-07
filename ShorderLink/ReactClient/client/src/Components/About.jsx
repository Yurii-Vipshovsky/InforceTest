import axios from "axios";
import { SERVER_URL } from "../Constants";
import { getCookie } from "../Services/CookieService";

function About(){
    axios.get(SERVER_URL+'Home/About',{
        headers: {
            'Authorization': `Bearer ${getCookie('token')}`
        },
    }
    )
    .then(function (response) {
        document.getElementById('aboutPage').innerHTML = response.data;
    })
    .catch(function (error) {
        console.error(error);
    });

    return(
        <>
            <div id="aboutPage">
            </div>
        </>
    )
}

export default About;