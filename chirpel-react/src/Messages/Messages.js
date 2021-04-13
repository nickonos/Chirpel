import React from "react"
import Message from "./Message";

class Messages extends React.Component {
    constructor(props) {
        super(props);
    }
    render() {
        return (
            <div class={"p-jc-between p-ai-center textAlign-left"}>
                {this.props.MessageList.map(message => {
                    return <div><Message message={message}/><br/></div>
                })}

            </div>
        )
    }
}
export default Messages