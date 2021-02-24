import './App.css';
import React from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import Header from "./Header"
import {Route, BrowserRouter as Router} from "react-router-dom"
const axios = require('axios');

const api = axios.create({
  baseURL: 'https://localhost:44380',
  timeout: 5000
});

function App() {
  let outp = '';
  api.get("/User")
      .then(res =>{
        outp = res.data
      });
  return (
    <div className="App">
        <Router>
            <Header />
                <body>
                     <p>
                         test
                    </p>
                </body>
            <Route path="/account/login"/>
        </Router>
    </div>

  )
}

export default App;
