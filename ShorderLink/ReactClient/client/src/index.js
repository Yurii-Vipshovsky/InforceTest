import React from 'react';
import ReactDOM from 'react-dom/client';
import './Styles/index.css';
import App from './App';
import reportWebVitals from './reportWebVitals';
import { createBrowserRouter, RouterProvider } from "react-router-dom";
import Login from './Components/Login';
import Register from './Components/Register';
import Links from './Components/Links';
import ShowInfoTab from './Components/ShowInfoTab';
import About from './Components/About';
import 'bootstrap/dist/css/bootstrap.css';

const root = ReactDOM.createRoot(document.getElementById('root'));

const router = createBrowserRouter([
  {
    path: "/",
    element: <App></App>,
    //errorElement: <NotFound></NotFound>,
  },
  {
    path: "login",
    element: <Login></Login>,
  },
  {
    path: "register",
    element: <Register></Register>,
  },
  {
    path: "all-links",
    element: <Links></Links>,
  },
  {
    path: "show-info/:value",
    element: <ShowInfoTab></ShowInfoTab>,
  },
  {
    path: "home/about",
    element: <About></About>,
  },
]);

root.render(
  <React.StrictMode>
    <RouterProvider router={router} />
  </React.StrictMode>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
