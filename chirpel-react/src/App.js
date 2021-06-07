import './App.css';
import React from 'react';
import {Route, BrowserRouter as Router, Switch, Redirect} from "react-router-dom";
import AuthPage from "./Pages/AuthPage"
import HomePage from "./Pages/HomePage"
import 'primereact/resources/themes/saga-blue/theme.css';
import 'primereact/resources/primereact.min.css';
import 'primeicons/primeicons.css';
import 'primeflex/primeflex.css';

const axios = require('axios');

const api = axios.create({
    baseURL: 'https://i468166core.venus.fhict.nl/',
    timeout: 10000

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
            <div>
                <Router>
                    <Switch>
                        <Route path="/auth" exact>
                            {this.state.loggedIn ? <Redirect to="/"/> : <AuthPage SetLogin={this.SetLogin} api={api} />}
                        </Route>
                        <Route path="/" exact>
                            {this.state.loggedIn ? <HomePage loggedin={this.state.loggedIn} api={api} accountId={this.state.accountId} SetLogin={this.SetLogin} /> : <Redirect to="/auth"/>}
                        </Route>
                        <Route path="/home" exact>
                            {this.state.loggedIn ? <HomePage loggedin={this.state.loggedIn} api={api} accountId={this.state.accountId} SetLogin={this.SetLogin} /> : <Redirect to="/auth"/>}
                        </Route>
                        <Route>
                            {this.state.loggedIn ? <HomePage loggedin={this.state.loggedIn} api={api} accountId={this.state.accountId} SetLogin={this.SetLogin} /> : <AuthPage SetLogin={this.SetLogin} api={api}/>}
                        </Route>
                    </Switch>
                </Router>
            </div>

        )
    }
}
export default App;
