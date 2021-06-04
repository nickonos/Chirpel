import React from 'react';
import Message from "./Message";

class Post extends React.Component{
    constructor(props) {
        super(props);
        this.state={
            message: {}
        }
    }

    componentDidMount(){
        this.props.api.get("/post/" + this.props.id)
            .then(m => {
                this.setState({message: m.data})
                console.log(m.data);
            })


    }

    render() {
        if(this.state.message !== null && this.state.message !== undefined){
            console.log(this.state.message)
            return<div style={{alignItems : "center", justifyContent: "center", display: "flex", textAlign: "left", marginTop: "20px"}} ><Message message={this.state.message}/></div>
        }
        return <span> 404 - Error message not found</span>

    }


}
export default Post