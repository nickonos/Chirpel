import React from "react";
import {Menu} from "primereact/menu";
import {BrowserRouter, Redirect} from "react-router-dom";
import {useHistory} from 'react-router-dom'


class NavMenu extends React.Component{
    constructor(props) {
        super(props);
        this.routeChange = this.routeChange.bind(this)
        this.items = [
            {
                style: {marginTop:"5px", marginLeft:"30px", fontWeight:"bold"},
                label:'Chirpel',
                command:(event) =>{
                    window.location ="/home";
                }
            },
            {separator: true},
            {
                label:'Explore',
                icon:'pi pi-fw pi-compass',command:(event) =>{
                    window.location = "/explore"
                }},
            {
                label:'Personal',
                icon:'pi pi-fw pi-inbox',command:(event) =>{
                    window.location = "/personal"
                }},
            {
                label: 'Profile',
                icon:"pi pi-fw pi-user", command:(event) =>{
                    window.location.href ="/profile/" + this.props.accountId;
                }},
            {
                label: 'Settings',
                icon: "pi pi-fw pi-cog",command:(event) =>{
                    window.location.href ="/settings";
                }},
            {
                label: 'Help',
                icon: "pi pi-fw pi pi-question",command:(event) => {
                    window.location.href = "/help";
                }},
            {
                separator:true,
            },
            {
                label: 'Log out',
                icon: "pi pi-fw pi-sign-out", command:(event) =>{
                    this.LogOut("/personal");
                }},
        ];
    }



    routeChange (path){
        let history = useHistory();
        history.push(path);
    }

    LogOut = () => {
        localStorage.removeItem("token")
        window.location.reload(true);
    }

    render() {
        return<div style={{position: "fixed",height:"100%", top:"0"}}>
            <Menu style={{padding: "0px",height:"100%"}} model={this.items}/>
        </div>
    }
}
export default NavMenu