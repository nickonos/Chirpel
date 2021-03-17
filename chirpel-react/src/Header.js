import React from "react"

import { Menubar } from 'primereact/menubar';
import {Image} from "react-bootstrap";
import {Avatar} from "primereact/avatar";
import {AutoComplete} from "primereact/autocomplete";


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

                }},

        ]
    }
];

class Header extends React.Component{
    constructor(props) {
        super(props);
    }

    log = () =>{
        console.log("test");
    }

    end = () =>{
        return(
            <span>
                <AutoComplete placeholder={"Username"} style={{margin: "5px", verticalAlign:"middle"}}></AutoComplete>
            {this.props.account.profilePicture !== undefined ?
                <Avatar image={require("./pictures/" + this.props.account.profilePicture)} shape={"circle"} style={{margin: "5px", verticalAlign:"middle"}}/> :
                <Avatar image={require("./pictures/Default.jpg")} shape={"circle"} style={{margin: "5px", verticalAlign:"middle"}} /> }
        </span>)
    }

    render(){
        console.log(this.props.account.profilePicture)
        return(
            <Menubar style={{maxHeight:"60px"}} model={items} end={this.end}></Menubar>
        )
    }
}


export default Header