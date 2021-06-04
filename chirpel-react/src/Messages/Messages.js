import React from "react"
import Message from "./Message";


class Messages extends React.Component {
    constructor(props) {
        super(props);

    }

    render() {
        return (
            <div style={{alignItems: "center"}}>
                <div class={"textAlign-left"}>
                    {this.props.MessageList ? this.props.MessageList.map(message => {
                        return <div style={{display: 'flex', justifyContent: 'center'}}><Message message={message}/>
                        </div>
                    }) : ""}
                </div>
            </div>
        )
    }
}
export default Messages