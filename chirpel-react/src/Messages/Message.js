import React from "react"
import {Avatar} from "primereact/avatar";
import {Link} from "react-router-dom";
import {DateTime} from "luxon";
import {Tooltip} from "primereact/tooltip";

class Message extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {
        var profilePicture;

        if (this.props.message.userpfp && this.props.message.userpfp !== ""){
            profilePicture = <Avatar image={require("./../pictures/"+ this.props.message.userpfp)} shape={"circle"}
                            style={{margin: "5px", verticalAlign: "middle"}}/>
        }else{
            profilePicture = <Avatar image={require("./../pictures/Default.jpg")} shape={"circle"}
                    style={{margin: "5px", verticalAlign: "middle"}}/>
        }

        return (
            <Link key={this.props.message.postId} to={"/post/" + this.props.message.postId} style={{width: "40%",textDecoration: 'none'}}>
                <div className={"message p-component"}>
                    <div style={{fontSize: "0.7em", color: "black"}}>
                        {profilePicture}
                        <Link className={"message-author"}
                              to={"/profile/" + this.props.message.userId}>@{this.props.message.username}</Link>
                        <span
                            className={"message-date"}
                            data-pr-tooltip={DateTime.fromISO(this.props.message.postDate,{zone: 'utc'} ).setLocale("nl").toLocaleString(DateTime.DATETIME_FULL)}> — {DateTime.fromISO(this.props.message.postDate,{zone: 'utc'}).toRelative({locale: "nl"})} gepost</span>
                    </div>
                    <div className={"p-d-flex p-jc-between p-ai-end"}>
                        <div style={{
                            marginTop: 5,
                            wordBreak: "break-all"
                        }} className={"message-content"}>{this.props.message.content}</div>
                        <div style={{
                            minWidth: 55,
                            textAlign: "right"
                        }}>
                            <div className={"message-comments"}> {this.props.message.comments || 0} <i className={"pi pi-comments"}/>
                            </div>
                            <div className={"message-comments"}> {this.props.message.likes || 0} <i className={"pi pi-heart"}/>
                            </div>
                        </div>
                    </div>

                    <Tooltip className={"tooltip"} target=".message-date" position={"bottom"}/>
                    </div>
                </Link>
        )
    }
}
export default Message