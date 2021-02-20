import './App.css';
import React from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import Header from "./Header"

const fetch = require('node-fetch');

function App() {
  return (
    <div className="App">
      <Header />
        <body >
        <p>
          Welcome to Chirpel
        </p>
        </body>
    </div>

  )
}

export default App;
