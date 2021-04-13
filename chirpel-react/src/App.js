import './App.css';
import React from 'react';
import {Route, BrowserRouter as Router, Switch, Redirect} from "react-router-dom";
import Auth from "./Auth"
import Home from "./Home"
import 'primereact/resources/themes/saga-blue/theme.css';
import 'primereact/resources/primereact.min.css';
import 'primeicons/primeicons.css';
import 'primeflex/primeflex.css';

const axios = require('axios');

const api = axios.create({
    baseURL: 'https://localhost:44380',
    timeout: 5000
});

 class App extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            loggedIn: false,
            accountId: ""
        }
    }

    componentDidMount() {
        if(localStorage.getItem("token") !== null){
            api.post('/user/verifyuser', {
                value: localStorage.getItem("token")
            })
                .then(res => {
                    if(res.data.succes){
                        this.setState({
                            loggedIn: true,
                            accountId: res.data.message
                        })
                    }
                })
        }
    }

     SetLogin = (loggedIn) => {
       this.setState({
           loggedIn: loggedIn
       })
    }

    render() {
        return (
                <Router>
                    <Switch>
                        <Route path="/auth" exact>
                            {this.state.loggedIn ? <Redirect to="/"/> : <Auth loggedin={this.SetLogin} api={api} />}
                        </Route>
                        <Route path="/" exact>
                            {this.state.loggedIn ? <Home loggedin={this.SetLogin} api={api} accountId={this.state.accountId} /> : <Redirect to="/auth"/>}
                        </Route>

                        <Route path="/home">
                            {this.state.loggedIn ? <Home loggedin={this.SetLogin} api={api} accountId={this.state.accountId} /> : <Redirect to="/auth"/>}
                        </Route>
                        <Route>
                            <Home loggedin={this.SetLogin} api={api} accountId={this.state.accountId} />
                        </Route>
                    </Switch>
                </Router>
        )
    }
}
export default App;
