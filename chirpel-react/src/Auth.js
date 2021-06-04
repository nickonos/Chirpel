import './App.css';
import React from 'react';
import {Button} from "primereact/button";
import {Sidebar} from "primereact/sidebar";
import { InputText } from "primereact/inputtext";
import {Password} from 'primereact/password';

class Auth extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            loggedIn: true,
            msg: null,
            loginPanel: false,
            registerPanel: false,
            username: "",
            loginUser: {
                username: "",
                password: ""
            },
            registerUser :{
                username: "",
                email: "",
                password: "",
                confirmpass: ""
            },
            usernameError: false,
            emailError: false,
            passwordError: false,
            confirmError: false
        }
    }

    Register = () => {
        this.resetError()
        if(this.checkFilled("register")){
            if (this.state.registerUser.password === this.state.registerUser.confirmpass){
                this.props.api.post('/user/register',{
                    Username: this.state.registerUser.username,
                    email: this.state.registerUser.email,
                    Password: this.state.registerUser.password
                }).then(res =>{
                    if(res.data.succes){
                        this.setState({
                            msg: "logging in"
                        })
                        this.setState({
                            loginUser:{
                                username: this.state.registerUser.username,
                                password: this.state.registerUser.password
                            }
                        })
                        this.LogIn();
                    }else{
                       this.registerErrorMessage(res.data.message)
                    }
                }).catch(error =>{
                       console.log(error)
                })
            }
            else{
                this.setState({
                    msg: "both passwords need to be the same",
                    confirmError: true,
                    passwordError: true
                })
            }
        }
    }

    LogIn = () => {
        this.resetError()
        if(this.checkFilled("login")){
            this.props.api.post('/user/login', {
                username: this.state.loginUser.username,
                password: this.state.loginUser.password
            }).then(res =>{
                    console.log(res)
                    if(res.data.succes){
                        localStorage.setItem("token", res.data.message)
                        this.props.loggedin(res.data.succes)
                    }
                    else{
                        this.loginErrorMessage(res.data.message)
                    }
                }).catch(error =>{
                    console.log(error)
            })
        }
    }

    SetLoginPanel = (visible) => {
        this.setState({
            loginPanel: visible,
            registerPanel: false
        })
        if(!visible){
            this.resetError()
        }
    }

    SetRegisterPanel = (visible) => {
        this.setState({
            registerPanel: visible,
            loginPanel: false
        })
        if(!visible){
            this.resetError()
        }
    }

    loginErrorMessage = (message)=>{
        if(message === 'username'){
            this.setState({
                usernameError: true,
                msg: "incorrect username"
            })
        }
        if(message === 'password'){
            this.setState({
                passwordError: true,
                msg: 'incorrect password for user "' + this.state.loginUser.username + '"'
            })
        }
    }

    registerErrorMessage = (message)=>{
        if(message === 'username'){
            this.setState({
                usernameError: true,
                msg: "username already in use"
            })
        }
        if(message === 'email'){
            this.setState({
                emailError: "email",
                msg: "email already in use"
            })
        }
        if(message === 'password'){
            this.setState({
                passwordError: true,
                msg: "password doesn't meet the requirements"
            })
        }
    }

    resetError =() =>{
        this.setState({
            msg: "",
            usernameError: false,
            emailError: false,
            passwordError: false,
            confirmError: false
        })
    }

    checkFilled = (user) =>{
        let succes = true;
        if (user === "register"){
            if(this.state.registerUser.username === ""){
                this.setState({
                    usernameError: true,
                    msg: "not all requiredfields filled"
                })
                succes = false
            }
            if(this.state.registerUser.email === ""){
                this.setState({
                    emailError: true,
                    msg: "not all requiredfields filled"
                })
                succes = false
            }
            if(this.state.registerUser.password === ""){
                this.setState({
                    passwordError: true,
                    msg: "not all requiredfields filled"
                })
                succes = false
            }
            if(this.state.registerUser.confirmpass === ""){
                this.setState({
                    confirmError: true,
                    msg: "not all requiredfields filled"
                })
                succes = false
            }
            return succes
        }
        if(user ==="login"){
            if(this.state.loginUser.username === ""){
                this.setState({
                    usernameError: true,
                    msg: "not all requiredfields filled"
                })
                succes = false
            }
            if(this.state.loginUser.password === ""){
                this.setState({
                    passwordError: true,
                    msg: "not all requiredfields filled"
                })
                succes = false
            }
            return succes
        }
    }

    render(){
        return (
            <div>
                <div className={"p-grid"} style={{overflow: "hidden"}}>

                    <div className="p-col-3 p-p-0" >

                    </div>

                    <div className="p-col-6 p-p-0 Center" style={{overflow: "hidden"}}>
                        <h1 style={{fontSize:"100px", marginTop:"30%", marginBottom:"5%"}}>Chirpel</h1>
                        <Button label="Login" style={{width:"10em"}} className={"p-button-rounded"}
                                onClick={() => this.SetLoginPanel(true)}/><br/>
                        <Button label="Register" style={{width:"10em", backgroundColor:"white"}} className={"p-button-rounded p-button-outlined p-mt-1" }
                                onClick={() => this.SetRegisterPanel(true)}/>
                    </div>
                    <div className="p-col-3 p-p-0">

                    </div>

                    <Sidebar style={{textAlign: "center", JustifyContent:"center", verticalAlign: "center"}} visible={this.state.loginPanel} position="right" onHide={() => this.SetLoginPanel(false)}>
                        <div>
                            <h1>Login</h1>
                            <span >
                                <InputText placeholder="Username" className={this.state.usernameError ? "p-invalid" : "" } keyfilter={"alphanum"} onChange={(e) =>
                                    this.setState({loginUser: { username: e.target.value, password : this.state.loginUser.password }})} />

                            </span>

                            <span className="p-mt-2 p-float-label">
                        <Password placeholder="Password" className={this.state.passwordError ? "p-invalid" : "" } feedback={false} onChange={(e) =>
                            this.setState({loginUser: { password: e.target.value, username: this.state.loginUser.username }})} />
                        <small className="p-error p-d-block">{this.state.msg}</small>
                    </span>

                            <Button type="button" label="Login" className="p-button-rounded p-mt-2" style={{ marginRight: '.25em' }} onClick={()=> this.LogIn()}/>

                        </div>
                    </Sidebar>

                    <Sidebar style={{textAlign: "center", JustifyContent:"center"}} visible={this.state.registerPanel} position="right" onHide={() => this.SetRegisterPanel(false)}>
                        <div>
                            <h1>Register</h1>
                            <span >
                            <InputText placeholder="Username" className={this.state.usernameError ? "p-invalid" : "" } keyfilter={"alphanum"}  onChange={(e) =>
                                this.setState({registerUser: { username: e.target.value, confirmpass: this.state.registerUser.confirmpass,
                                        email: this.state.registerUser.email, password: this.state.registerUser.password}})} />
                        </span>

                            <span className="p-mt-2 p-float-label">
                            <InputText placeholder="example@mail.com" className={this.state.emailError ? "p-invalid" : "" } onChange={(e) =>
                                this.setState({registerUser: { email: e.target.value, username: this.state.registerUser.username,
                                        confirmpass: this.state.registerUser.confirmpass, password: this.state.registerUser.password}})} />
                        </span>

                            <span className="p-mt-2 p-float-label">
                            <Password placeholder="Password" className={this.state.passwordError ? "p-invalid" : "" } feedback={false} onChange={(e) =>
                                this.setState({registerUser: { password: e.target.value, username: this.state.registerUser.username,
                                        email: this.state.registerUser.email, confirmpass: this.state.registerUser.confirmpass}})} />
                        </span>

                            <span className="p-mt-2 p-float-label">
                            <Password placeholder="Confirm password" feedback={false} className={this.state.confirmError ? "p-invalid" : "" } onChange={(e) =>
                                this.setState({registerUser: { confirmpass: e.target.value, username: this.state.registerUser.username,
                                        email: this.state.registerUser.email, password: this.state.registerUser.password}})} />
                                        <small className="p-error p-d-block">{this.state.msg}</small>
                        </span>
                        </div>

                        <Button type="button" label="Register" className="p-button-rounded p-mt-2" style={{ marginRight: '.25em' }} onClick={()=> this.Register()}/>
                    </Sidebar>
                </div>
            </div>
        )
    }
}
export default Auth