import React from "react"

import { Menubar } from 'primereact/menubar';
import {Avatar} from "primereact/avatar";
import {AutoComplete} from "primereact/autocomplete";
import {Button} from "primereact/button";

class Header extends React.Component{
    constructor(props) {
        super(props);
    }

    end = () =>{
        if(this.props.loggedin === true){
            return(
                <span>
                <AutoComplete placeholder={"Username"} style={{margin: "5px", verticalAlign:"middle"}}/>
                    {this.props.account.profilePicture !== undefined && this.props.account.profilePicture !== null && this.props.account.profilePicture !== "" ?
                        <Avatar image={require("./pictures/" + this.props.account.profilePicture)} shape={"circle"} style={{margin: "5px", verticalAlign:"middle"}}/> :
                        <Avatar image={require("./pictures/Default.jpg")} shape={"circle"} style={{margin: "5px", verticalAlign:"middle"}} /> }
        </span>)
        }else{
            return(
                <span>
                    <Button label="Login" style={{width:"6em", margin:"5px"}} className={"p-button-rounded p-button-sm"} onClick={this.Login}/>
                    <Button label="Register" style={{width:"6em",margin:"5px"}} className={"p-button-rounded p-button-sm p-button-outlined"} onClick={this.Login}/>
                </span>)
        }
    }

    Login(){
        window.location ="/auth";
    }

    render(){
        return(
            <Menubar style={{minHeight:"50px",padding: "0px"}}  end={this.end}/>
        )
    }
}


export default Header