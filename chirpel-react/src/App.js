import './App.css';
import React from 'react';
import {Route, BrowserRouter as Router, Switch, Redirect} from "react-router-dom";
import Auth from "./Auth"
import Home from "./Home"
import 'primereact/resources/themes/saga-blue/theme.css';
import 'primereact/resources/primereact.min.css';
import 'primeicons/primeicons.css';
import 'primeflex/primeflex.css';

 class App extends React.Component {
    constructor(props) {
        super(props);

        this.state ={
         loggedIn: false
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
                        <Route path="/" exact>
                            {this.state.loggedIn ? <Home /> : <Redirect to="/auth"/>}
                        </Route>
                        <Route path="/auth" exact>
                            {this.state.loggedIn ? <Redirect to="/"/> : <Auth loggedin={this.SetLogin} />}
                        </Route>
                        <Route path="/home">
                            {this.state.loggedIn ? <Home/> : <Redirect to="/auth"/>}
                        </Route>
                        <Route>
                            <Home />
                        </Route>
                    </Switch>
                </Router>
        )
    }
}
export default App;
