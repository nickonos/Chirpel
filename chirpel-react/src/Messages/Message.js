import React from "react"
import {Card} from "primereact/card";
import {Avatar} from "primereact/avatar";

class Message extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <div>
                <Card subTitle={this.props.message.username} style={{width: "500px"}}
                      className={"p-card-content-invalid"}>{this.props.message.content} {this.props.message.userpfp !== undefined && this.props.message.userpfp !== "" ?
                    <Avatar image={require("src/pictures/"+ this.props.message.userpfp)} shape={"circle"}
                            style={{margin: "5px", verticalAlign: "middle"}}/> :
                    <Avatar image={require("src/pictures/Default.jpg")} shape={"circle"}
                            style={{margin: "5px", verticalAlign: "middle"}}/>}</Card>
            </div>
        )
    }
}
export default Message