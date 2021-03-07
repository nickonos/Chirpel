import React from "react"

import { Menubar } from 'primereact/menubar';
import { InputText } from 'primereact/inputtext';
const items = [
    {
        icon:'pi pi-home',
    },
    {
        icon:'pi pi-fw pi-users',
    },
    {
        label:'Events',
        icon:'pi pi-fw pi-calendar',
        items:[
            {
                label:'Edit',
                icon:'pi pi-fw pi-pencil',
                items:[
                    {
                        label:'Save',
                        icon:'pi pi-fw pi-calendar-plus'
                    },
                    {
                        label:'Delete',
                        icon:'pi pi-fw pi-calendar-minus'
                    },

                ]
            },
            {
                label:'Archieve',
                icon:'pi pi-fw pi-calendar-times',
                items:[
                    {
                        label:'Remove',
                        icon:'pi pi-fw pi-calendar-minus'
                    }
                ]
            }
        ]
    },
    {
        label:'Quit',
        icon:'pi pi-fw pi-power-off'
    }
];

const start = <img alt="logo" src={require("./logo-rounded.png")} onError={(e) => e.target.src='https://www.primefaces.org/wp-content/uploads/2020/05/placeholder.png'} height="40" className="p-mr-2"></img>;
const end = <div>
    <i className={"pi pi-user"}/>
    <span style={{margin: "5px"}}>Username</span>
</div>

class Header extends React.Component{

    render(){
        return(
            <Menubar model={items} start={start} end={end}></Menubar>
        )
    }
}


export default Header