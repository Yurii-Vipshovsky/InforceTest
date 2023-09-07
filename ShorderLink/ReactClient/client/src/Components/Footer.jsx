import { Link } from "react-router-dom";

function Footer(){
    return(
        <footer class="border-top footer text-muted">
            <div class="container">
                &copy; 2023 - ShorterLink - <Link to={'home/about'}>About</Link>
            </div>
        </footer>
    );
}

export default Footer;