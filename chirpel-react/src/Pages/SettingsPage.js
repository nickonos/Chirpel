import React from "react";
import {Button} from "primereact/button";

class SettingsPage extends React.Component{
    constructor(props) {
        super(props);
        this.state={
            test: false
        }
    }

    DeleteUser(){
        console.log("delete" + localStorage.getItem("token"))
        this.props.api.post("/user/delete", {
            Value: localStorage.getItem("token")
        }).then(res => {
            if(res.data.succes){
                localStorage.removeItem("token")
                window.location.href ="/auth"
            }})
    }

    render() {
        return(<div>
            <h1>Settings</h1>
            <Button className={"p-button-rounded p-button-danger"} onClick={() => this.DeleteUser()}>Delete User</Button>
        </div>)
    }
}
export default SettingsPage