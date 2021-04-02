import React from "react"
import Message from "./Message";

class Messages extends React.Component {
    constructor(props) {
        super(props);
    }
    render() {
        return (
            <div>
                <Message message={this.props.MessageList[0]}></Message><br/>
                <Message message={this.props.MessageList[1]}></Message><br/>
                <Message message={this.props.MessageList[2]}></Message><br/>
            </div>
        )
    }
}
export default Messages