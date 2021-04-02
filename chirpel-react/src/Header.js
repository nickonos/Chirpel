import React from "react"

import { Menubar } from 'primereact/menubar';
import {Avatar} from "primereact/avatar";
import {AutoComplete} from "primereact/autocomplete";

const LogOut = () => {
    localStorage.removeItem("token")
}

const items = [
    {
        icon:'pi pi-fw pi-home',
    },
    {
        icon:'pi pi-fw pi-compass',
    },
    {
        icon:'pi pi-fw pi-inbox',
    },
    {
        icon:'pi pi-fw pi-cog',
        items: [
            {label: 'Profile', command:(event) =>{
                    window.location ="/profile";
                }},
            {label: 'Settings', command:(event) =>{
                window.location ="/settings";
                }},
            {label: 'Help',command:(event) =>{
                    window.location ="/help";
                }},
            {
                separator:true
            },
            {label: 'Log out', command:(event) =>{
                LogOut()
                }},

        ]
    }
];

class Header extends React.Component{
    constructor(props) {
        super(props);
    }

    end = () =>{
        return(
            <span>
                <AutoComplete placeholder={"Username"} style={{margin: "5px", verticalAlign:"middle"}}></AutoComplete>
            {this.props.account.profilePicture !== undefined && this.props.account.profilePicture !== "" ?
                <Avatar image={require("./pictures/" + this.props.account.profilePicture)} shape={"circle"} style={{margin: "5px", verticalAlign:"middle"}}/> :
                <Avatar image={require("./pictures/Default.jpg")} shape={"circle"} style={{margin: "5px", verticalAlign:"middle"}} /> }
        </span>)
    }

    render(){
        return(
            <Menubar style={{maxHeight:"60px"}} model={items} end={this.end}></Menubar>
        )
    }
}


export default Header