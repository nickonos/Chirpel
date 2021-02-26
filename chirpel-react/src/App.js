import './App.css';
import React from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import Header from "./Header"
import {Button, Card, Form} from "react-bootstrap";
import {Route, BrowserRouter as Router} from "react-router-dom"
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
            msg: null,
            loginPanel: false,
            registerPanel: false
        }
    }


    Register = () => {
        api.post('/user/register',{
            Username: 'nick',
            email: 'nick@gmail.com',
            Password: 'nick'
        }).then(res =>{
            console.log("test")
            this.setState({
                loggedIn: res.data.succes,
                msg: res.data.message
            })

        }).catch(error =>{
            this.setState({
                msg: error.message
            })
        })
    }

    LogIn = (values) => {
        api.post('/user/login',{
            Username: 'nick',
            Password: 'nick'
        }).then(res =>{
            console.log("test")
            this.setState({
                loggedIn: res.data.succes,
                msg: res.data.message
            })

        }).catch(error =>{
            this.setState({
                msg: error.message
            })
        })
    }

    ShowLoginPanel = () =>{
        this.setState({
            registerPanel: false,
            loginPanel: true
        })
    }

    ShowRegisterPanel = () =>{
        this.setState({
            loginPanel: false,
            registerPanel: true
        })
    }

    HideLoginPanel = () =>{
        this.setState({
            loginPanel: false
        })
    }

    HideRegisterPanel = () =>{
        this.setState({
            registerPanel: false
        })
    }

    ShowPanel = () =>{
        if(this.state.loginPanel){
            return <Card style={{float:"right", height:"1000px", width:"400px", backgroundColor:"lightgray  ", borderColor:"darkgray", borderWidth:"3px"}}>
                <Card.Title>
                    <Button variant="outline-dark" size="sm" style={{float:"right"}} onClick={this.HideLoginPanel}>X</Button>
                </Card.Title>
                <Card.Body style={{marginTop:"80%", marginBottom:"80%"}}>
                    <Form>
                        <Form.Group>
                            <Form.Label>Login</Form.Label>
                            <Form.Control type="text" placeholder="Username" />
                            <Form.Text className="text-muted"/>
                        </Form.Group>
                        <Form.Group>
                            <Form.Control type="password" placeholder="password" />
                            <Form.Text className="text-muted"/>
                        </Form.Group>
                    </Form>
                    <Button variant="secondary" style={{margin:"5px", width:"100%"}} onClick={this.LogIn}>Login</Button>
                </Card.Body>
            </Card>
        }
        if(this.state.registerPanel){
            return <Card style={{float:"right", height:"1000px", width:"400px", justifyContent:"center", backgroundColor:"lightgray  ", borderColor:"darkgray", borderWidth:"3px"}}>
                <Card.Title>
                    <Button variant="outline-dark" size="sm" style={{float:"right"}} onClick={this.HideRegisterPanel}>X</Button>
                </Card.Title>
                <Card.Body style={{marginTop:"300px"}}>
                    <Form>
                        <Form.Group>
                            <Form.Label>Register</Form.Label>
                            <Form.Control type="text" placeholder="Username" />
                            <Form.Text className="text-muted"/>
                        </Form.Group>
                        <Form.Group>
                            <Form.Control type="email" placeholder="Name@example.com" />
                            <Form.Text className="text-muted"/>
                        </Form.Group>
                        <Form.Group>
                            <Form.Control type="password" placeholder="Password" />
                            <Form.Text className="text-muted"/>
                        </Form.Group>
                        <Form.Group>
                            <Form.Control type="password" placeholder="Confirm password" />
                            <Form.Text className="text-muted"/>
                        </Form.Group>
                    </Form>
                    <Button variant="secondary" style={{margin:"5px", width:"100%"}} onClick={this.Register}>Register</Button>
                </Card.Body>
            </Card>
        }
    }


    render() {
        if (
            this.state.loggedIn) {
            document.body.style.backgroundColor = "gray"
            return (
                <div className="App">
                    <Router>
                        <Header/>
                            <body style={{backgroundColor: "gray", marginLeft: "10px"}}>
                                <p>
                                    test
                                </p>
                            </body>
                        <Route path="/account/login"/>
                    </Router>
                </div>
            )
        } else {
            document.body.style.backgroundColor = "gray"
            return (
                <div>
                    {this.ShowPanel()}
                        <div className="App" style={{
                            display:"flex",
                            justifyContent: "center",
                            alignItems: "center",
                            position:"absolute",
                            left:"40%",
                            top:"20%"
                        }}>
                            <body style={{backgroundColor: "gray",}}>
                                <h1 style={{fontSize: "100px"}}>Chirpel</h1>
                                <Button variant="secondary" style={{width: "80px", marginBottom: "10px", marginTop: "100px", marginLeft: "130px"}}
                                    onClick={this.ShowLoginPanel}>Login</Button><br/>
                                 <Button variant="outline-dark"
                                    style={{width: "80px",marginLeft: "130px", backgroundColor: "white"}}
                                    onClick={this.ShowRegisterPanel}>Register</Button>
                            </body>
                        </div>
                </div>
            )
        }
    }
}
export default App;
