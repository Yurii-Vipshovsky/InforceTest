import React, { useEffect, useState } from 'react';
import axios from "axios";
import { SERVER_URL } from '../Constants';
import AddLink from './AddLink';
import Header from './Header';
import Footer from './Footer';
import '../Styles/links.css';
import { getCookie } from '../Services/CookieService';
import { Link } from "react-router-dom";

function Links(){
    const [links, setLinks] = useState([]);
    const [isUserLogined, setIsUserLoggined] = useState(false);
    const [currentUserId, setCurrentUserId] = useState("");
    const [isUserAdmin, setIsUserAdmin] = useState(false);

    async function fetchLinks() {
        try{
        const result = await axios.get(SERVER_URL+"Links/GetAllLinks");
        setLinks(result.data);
        }
        catch{
            console.log('No data From Server');
        }
    }

    const parseJwt = (token) => {
        try {
          return JSON.parse(atob(token.split('.')[1]));
        } catch (e) {
          return null;
        }
    };

    useEffect(() => {
        fetchLinks();
        let userData = parseJwt(getCookie('token'));
        if(userData!=null){
            setCurrentUserId(userData['userId']);
            if(userData['userRole'] === 'admin'){
                setIsUserAdmin(true);
            }
        }
        if(getCookie('token')==null){
            setIsUserLoggined(false);
        }
        else{
            setIsUserLoggined(true);
        }
        
    },[])

    const handleRefreshLinks = () => {
        fetchLinks();
    };

    async function handleDeleteClick(linkId){
        console.log(linkId);
        try{
            console.log("start delete");
            await fetch(SERVER_URL+'Links/Delete/'+linkId,{
                method:"POST",
                headers: {
                    'Authorization': `Bearer ${getCookie('token')}`
                },
            }).then(setLinks(links.filter((link) => link.id !== linkId)))
        }
        catch(err){
            console.log(err);
        }
    }

    return(
        <>
            <Header/>
            <div className="links">
                <table className="table table-striped" aria-labelledby="tableLabel">
                    <thead>
                        <tr>
                            <th>Short Link</th>
                            <th>Original Link</th>
                            {isUserLogined  && <th>Info</th>}
                            {isUserLogined && <th>Delete</th>}
                        </tr>
                    </thead>
                    <tbody>
                        {links.map(link =>
                            <tr key={link.id}>
                                <td><a href={link.shortLink}>{link.shortLink}</a></td>
                                <td><a href={link.originalLink}>{link.originalLink}</a></td>
                                {(isUserAdmin || link.creatorId===currentUserId) && <td><Link to={'/show-info/'+link.id}><button type="button" class="btn btn-info">Info</button></Link></td>}
                                {(isUserAdmin || link.creatorId===currentUserId) && <td><button type="button" class="btn btn-danger" onClick={()=>handleDeleteClick(link.id)}>Delete</button></td>}
                            </tr>
                        )}
                    </tbody>
                </table>
                <div>
                    {isUserLogined && <AddLink onAddLink={handleRefreshLinks}/>}
                </div>
            </div>
            <Footer/>
        </>
    );
}

export default Links;