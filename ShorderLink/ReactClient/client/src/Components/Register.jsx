import { SERVER_URL } from "../Constants";
import Footer from "./Footer";
import Header from "./Header";
import '../Styles/login-register.css';

function Register(){
    async function submitRegister(){
        try{
            const data = new FormData();
            data.append("name", document.querySelector('input[name="name"]').value);
            data.append("email", document.querySelector('input[name="email"]').value);
            data.append("password", document.querySelector('input[name="password"]').value);
            await fetch(SERVER_URL+"users/register",{
                method:"POST",
                body: data
            })
            .then(response => {
                if (!response.ok) {
                    document.getElementById('registerFail').classList.remove('hidden')
                }
                else{
                    document.getElementById('registerSuccess').classList.remove('hidden');
                    document.getElementById('registerFail').classList.add('hidden');
                }
            })
        }
        catch(err){
            console.log(err);
        }
    }

    return(
        <>
            <Header/>
            <div className="login-register">
                <label>User Name</label><input required name="name"></input>
                <label>Email</label><input required type="email" name="email"></input>
                <label>Password</label><input required name="password"></input>
                <button type="button" className="btn btn-light" onClick={submitRegister}>Submit</button>
                <h3 id="registerSuccess" className="login-register__successful hidden">New User Registered</h3>
                <h3 id="registerFail" className="login-register__fail hidden">User with same email already registered</h3>
            </div>
            <Footer/>
        </>
    );
}

export default Register;