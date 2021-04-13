import React from "react"
import Messages from "../Messages/Messages";

class ExplorePage extends React.Component{
    constructor(props) {
        super(props);
        this.state={
            MessageList : []
        }
    }
    componentDidMount() {
        this.props.api.get('/post/personal')
            .then(res =>{
                console.log(res)
                this.setState({MessageList : res.data})
            })
    }

    render() {
        return(
            <div>
                {this.state.MessageList[0] !== undefined ? <Messages  MessageList={this.state.MessageList}></Messages> : <span>no messages found</span>}

            </div>)
    }
}
export default ExplorePage