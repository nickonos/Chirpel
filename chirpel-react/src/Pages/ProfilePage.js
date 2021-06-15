import React from "react";
import {Link} from "react-router-dom";
import Message from "../Messages/Message";
import {Divider} from "primereact/divider";
import {InputTextarea} from "primereact/inputtextarea";
import {Avatar} from "primereact/avatar";
import {Button} from "primereact/button";
import './ProfilePage.css';

class ProfilePage extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            account: {
                profilePicture: "Default.jpg",
                accountId: this.props.accountId,
                bio: "",
                username: "",
                followers: [],
                following: [],
                posts: [],
                isPrivate: false
            },
            editing: false,
            FollowingUser: false,
            tempBio: ""
        }
    }

    componentDidMount() {
        this.props.api.get("/User/" + this.props.id)
            .then(res => {
                this.setState({
                    account: {
                        accountId: res.data.id,
                        bio: res.data.bio,
                        username: res.data.username,
                        followers: res.data.followers,
                        following: res.data.following,
                        posts: res.data.posts,
                        isPrivate: res.data.isPrivate,
                        FollowerCount: res.data.followers.length,
                        FollowingCount: res.data.following.length,
                    },
                    tempBio : res.data.bio
                })
                if(res.data.followers.find(c => c === this.props.accountId) !== undefined){
                    this.setState({
                        FollowingUser: true
                    })
                }
            })
    }

    setValue(value){
        this.setState({
            tempBio: value
        })
    }

    toggleEditing() {
        if(this.state.editing){
            this.props.api.post("user/settings/bio/",{
                Token : localStorage.getItem("token"),
                Bio : this.state.tempBio
            })
                .then(res => {
                    console.log(res)
                })
        }
        this.setState({
            editing: !this.state.editing
        })
    }

    UnFollow(){
        this.props.api.post("user/unfollow/"+this.state.account.accountId, {Value: localStorage.getItem("token")}).then(res =>{
            if(res.data.succes){
                this.setState({
                    FollowingUser : false
                })
            }
        })
    }

    Follow(){
        this.props.api.post("user/follow/"+this.state.account.accountId, {Value: localStorage.getItem("token")}).then(res =>{
            if(res.data.succes){
                this.setState({
                    FollowingUser : true
                })
            }
        })
    }

    render() {
        return (<div style={{justifyContent: "center", alignItems: "center", display: "flex"}}>
                {this.state.account.username ?
                    <div style={{width: "40%"}}>
                        <div className="profile-info">
                            <div className="profile-name">
                                <div className="profile-name p-d-flex p-jc-between" style={{width: "100%"}}>
                                    <div className="account-name">
                                        {<Avatar image={require("../pictures/Default.jpg")} shape={"circle"} style={{
                                            margin: "5px",
                                            verticalAlign: "middle",
                                            height: "100px",
                                            width: "100px"
                                        }}/>}
                                    </div>
                                    <div style={{float: "left"}}>
                                        <h1 style={{marginTop: "0px", float: "left"}}>{this.state.account.username}</h1>
                                        <br/>
                                        <span style={{}}>Posts: {this.state.account.posts.length}</span>
                                    </div>
                                    <div style={{marginTop: "3px"}}>
                                        <br/><br/><br/>
                                        <span
                                            style={{bottom: "0"}}>Followers: {this.state.account.followers.length}</span>
                                    </div>

                                    <div style={{position: "relative"}}>
                                        {this.state.account.accountId !== this.props.accountId ?<div>
                                                {this.state.FollowingUser ? <Button className="p-button-secondary p-button-text" icon={"pi pi-check"} iconPos="right" style={{marginBottom: "6px"}} onClick={() => this.UnFollow()}/>
                                                : <Button className="p-button-secondary p-button-text" icon={"pi pi-user-plus"} iconPos="right" style={{marginBottom: "6px"}} onClick={ () =>this.Follow()}/>}
                                            </div>
                                             : <div style={{marginBottom:"3px"}}> <br/> <br/> </div>}
                                        <br/>
                                        <span>Following: {this.state.account.following.length}</span>
                                    </div>
                                    <div style={{width: "80px"}}/>

                                </div>
                                <div>
                                    {this.props.accountId === this.state.account.accountId ?
                                        <Button onClick={() => {
                                            this.toggleEditing()
                                        }} className="p-button-text" label={this.state.editing ? "Save" : "Edit"}
                                                iconPos="right"
                                                icon={this.state.editing ? "pi pi-save" : "pi pi-pencil"}/> : ""}
                                </div>
                            </div>

                            {this.state.editing ? <div>
                                    <div>
                                    </div>
                                    <br/>

                                    <div><InputTextarea style={{width: "100%"}} value={this.state.tempBio}
                                                        onChange={(e) => this.setValue(e.target.value)}/></div>
                                    <br/>
                                </div>

                                :

                                <div style={{textAlign: "left", marginTop: "5px", marginLeft: "20px"}}>
                                    {this.state.account.bio && this.state.account.bio.length > 0 ? <div>
                                        {this.state.editing ? "" :
                                            <div style={{whiteSpace: "pre-wrap"}}>{this.state.account.bio}</div>}
                                    </div> : ""} <br/>
                                </div>}
                        </div>
                        <Divider align="left">
                        </Divider>
                        {
                            this.state.account.isPrivate ? <span>This account is private</span> :
                                <div className="messages" style={{textAlign: "left"}}>
                                    {this.state.account.posts.map(message => {
                                        return <Link key={message.id} style={{textDecoration: 'none'}}
                                                     to={"/thread/" + message.id}>
                                            <Message message={message}>
                                            </Message>
                                        </Link>
                                    })}
                                </div>
                        }

                    </div>
                    : <h1> Loading profile...</h1>}

            </div>

        )
    }
}
export default ProfilePage