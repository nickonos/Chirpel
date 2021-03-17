import Header from "./Header";
import React from "react";
import {Button} from "primereact/button";


class Home extends React.Component{
    constructor(props) {
        super(props);

        this.state ={
            account: {
                profilePicture : "Default.jpg",
                accountId: this.props.accountId,
                bio : "",
                username : "",
                followers : [],
                following : [],
                posts : [],
                isPrivate : false
            }
        }
    }

    LogOut = () => {
        localStorage.removeItem("token")
        this.setState({
            account: ""
        })
        this.props.loggedin(false)
    }


    componentWillMount(){
        if(this.state.accountId !== null){
            this.props.api.get('/user/'+this.props.accountId)
                .then(res =>{
                    this.setState({
                        account: {
                            profilePicture : res.data.profilePicture,
                            accountId: res.data.id,
                            bio : res.data.bio,
                            username : res.data.username,
                            followers : res.data.followers,
                            following : res.data.following,
                            posts : res.data.posts,
                            isPrivate : res.data.isPrivate
                        }
                    })
                })
        }
    }


    render() {
        return (
            <div className="App">
                    <Header account={this.state.account}/>
                    <Button onClick={this.LogOut}>token</Button>
            </div>
        )
    }
}
export default Home