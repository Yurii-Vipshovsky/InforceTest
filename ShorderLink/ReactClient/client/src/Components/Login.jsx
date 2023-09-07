import { setCookie } from "../Services/CookieService";
import { SERVER_URL } from "../Constants";
import Footer from "./Footer";
import Header from "./Header";
import '../Styles/login-register.css';

function Login(){

    async function submitLogin(){
        let userEmail = document.querySelector('input[name="email"]').value;
        let userPassword = document.querySelector('input[name="password"]').value;
        console.log(userEmail, userPassword);
        const data = new FormData();
        data.append("email", userEmail);
        data.append("password", userPassword);
        try{
            await fetch(SERVER_URL+"users/login",{
                method:"POST",
                body: data
            })
            .then(response => {
                if (!response.ok) {
                    document.querySelector('.login-register__fail').classList.remove('hidden')
                }
                else{
                    document.querySelector('.login-register__fail').classList.add('hidden');
                    return response.json();                    
                }
            })
            .then((result)=> {
                setCookie('token', result.access_token, 15);
                document.querySelector('.login-register__successful').classList.remove('hidden');
            });                        
        }
        catch(err){
            console.log(err);
        }
    }

    return(
        <>
            <Header/>
            <div className="login-register">
                <label>Email</label><input required name="email"></input>
                <label>Password</label><input required name="password"></input>
                <button type="button" class="btn btn-light" onClick={submitLogin}>Submit</button>
                <h3 className="login-register__successful hidden">Login Successful</h3>
                <h3 className="login-register__fail hidden">Login Failed</h3>
            </div>
            <Footer/>
        </>
    );
}

export default Login;