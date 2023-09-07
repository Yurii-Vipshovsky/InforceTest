import { Link } from "react-router-dom";
import '../Styles/header.css';
import { useState, useEffect } from "react";
import { getCookie, setCookie } from "../Services/CookieService";

function Header(){
    const [isUserLogined, setIsUserLoggined] = useState(false);
    
    useEffect(()=>{
        if(getCookie('token')==null){
            setIsUserLoggined(false);
        }
        else{
            setIsUserLoggined(true);
        }
    },[])

    function handleLogout(){
        setIsUserLoggined(false);
        setCookie('token', null, 1);
    }
    
    return(
        <div className="header">
            <div className="header__name-about-section">
                <h1>ShorterLinks Inforce</h1>
                <Link to={'/home/about'}>About</Link>
            </div>
            <Link to={'/all-links'}>All Links</Link>
            { !isUserLogined &&
            <div className="header__login-section">
                <Link to={'/register'}>Register</Link>
                <Link to={'/login'}>Login</Link>
            </div>}
            { isUserLogined &&
            <div className="header__login-section">
                <button type="button" class="btn btn-link"onClick={handleLogout}>Logout</button>
            </div>}
        </div>
    );
}

export default Header;