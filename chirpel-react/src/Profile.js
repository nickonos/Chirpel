import React from "react";

class Profile extends React.Component {
    constructor(props) {
        super(props);
        this.state ={
            account: {
                profilePicture: "Default.jpg",
                accountId: this.props.accountId,
                bio: "",
                username: "",
                followers: [],
                following: [],
                posts: [],
                isPrivate: false
            }
        }
    }

    componentDidMount() {
        this.props.api.get("/User/" + this.props.id)
            .then(res => {
                if(res.data.profilePicture !== null){
                    this.setState({
                        account: {
                            profilePicture: res.data.profilePicture,
                            accountId: res.data.id,
                            bio: res.data.bio,
                            username: res.data.username,
                            followers: res.data.followers,
                            following: res.data.following,
                            posts: res.data.posts,
                            isPrivate: res.data.isPrivate
                        }
                    })
                }else{
                    this.setState({
                        account: {
                            accountId: res.data.id,
                            bio: res.data.bio,
                            username: res.data.username,
                            followers: res.data.followers,
                            following: res.data.following,
                            posts: res.data.posts,
                            isPrivate: res.data.isPrivate
                        }
                    })
                }
            })


    }

    render() {
        console.log(this.state.account)
        return(
            <div style={{alignItems :"center", justifyContent: "center", display: "flex"}}>
                <div className={"message p-component"} style={{width: "40%", textAlign: "left"}}>
                    {this.state.account.username}
                </div>

            </div>
        )
    }
}
export default Profile