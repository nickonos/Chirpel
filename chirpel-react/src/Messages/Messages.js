import React from "react"
import Message from "./Message";
import {Button} from "primereact/button";
import {Sidebar} from "primereact/sidebar";
import {InputTextarea} from "primereact/inputtextarea";

class Messages extends React.Component {
    constructor(props) {
        super(props);

    }

    render() {
        return (
            <div style={{alignItems : "center"}}>
                <div class={"p-jc-between p-ai-center textAlign-left"}>
                    {this.props.MessageList ? this.props.MessageList.map(message => {
                        return <div style={{display: 'flex',  justifyContent:'center'}}><Message message={message}/></div>
                    }) : ""}
                </div>
            </div>
        )
    }
}
export default Messages