import React from "react"
import Messages from "../Messages/Messages";
import {Button} from "primereact/button";
import {Sidebar} from "primereact/sidebar";
import {InputTextarea} from "primereact/inputtextarea";

class ExplorePage extends React.Component{
    constructor(props) {
        super(props);
        this.state={
            MessageList : [],
            PostWindow : false,
            PostText : "",
            PostError: null
        }
    }
    componentDidMount() {
        this.props.api.get('/post/personal')
            .then(res =>{
                console.log(res)
                this.setState({MessageList : res.data})
            })
    }

    SetPostWindow =(state) =>{
        this.setState({PostWindow : state} )
    }

    SetPostText =(text) =>{
        this.setState({PostText : text})
        this.setState({PostError : null})
        if(text !== null && text.length >= 255){
            this.setState({PostError : "Post is too long"})
        }
    }

    SendCreatePost =() =>{
        if(this.state.PostText !== null && this.state.PostText.length < 255 && this.state.PostText.length > 1){
            this.props.api.post("/post/create", {
                Content : this.state.PostText,
                Token : localStorage.getItem("token")
            })
            this.SetPostWindow(false)
        }
    }

    render() {
        return(
            <div>
                {this.state.MessageList[0] !== undefined ? <Messages  MessageList={this.state.MessageList}></Messages> : <span>Loading messages...</span>}

                <div className={"p-float-right"}  style={{right: 20, bottom: 20, position: "fixed"}}>
                    <Button icon="pi pi-plus" className="p-button-rounded p-button-secondary" onClick={ () => this.SetPostWindow(!this.state.PostWindow)} />
                </div>

                <Sidebar visible={this.state.PostWindow} position="right" onHide={() => this.SetPostWindow(false)}>
                    <h1 style={{ fontWeight: 'normal' }}>Create Post</h1>
                    <InputTextarea rows={5} cols={30} value={this.state.PostText} onChange={(e) => this.SetPostText(e.target.value)} autoResize/>

                    <Button type="button" onClick={() =>this.SendCreatePost()} label="Post" className="p-button-sm" style={{ marginRight: '.25em' }} />
                    <small className="p-error p-d-block">{this.state.PostError}</small>
                </Sidebar>
            </div>)
    }
}
export default ExplorePage