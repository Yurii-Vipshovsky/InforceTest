import { SERVER_URL } from '../Constants';
import { getCookie } from "../Services/CookieService";

function AddLink({ onAddLink }){

    async function sendData() {
        try{
            let linkToShort = document.querySelector('input[name="linkToShort"]').value;
            await fetch(SERVER_URL+'links/create', {
                method: 'POST',
                headers: {
                    'Authorization': `Bearer ${getCookie('token')}`,
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ 'OriginalLink': linkToShort })
            })
            .then(()=>onAddLink())
            .catch(console.log('catch'))
        }
        catch{
            document.getElementById('errorMessage').classList.remove('hidden');
        }
    }
    
    function hideErrorMessage(){
        let errorMessage = document.getElementById('errorMessage');
        if(!errorMessage.classList.contains('hidden')){
            errorMessage.classList.add('hidden');
        }        
    }

    return (
        <div>
            <h1 id="tableLabel">Create Short Link</h1>
            <input onChange={hideErrorMessage} name='linkToShort'></input>
            <button type="button" className="btn btn-light" onClick={sendData}>Create</button>
            <h5 id='errorMessage' className='hidden'>Link already exists</h5>
        </div>
    );
}

export default AddLink;