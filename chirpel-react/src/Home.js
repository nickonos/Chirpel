import Header from "./Header";
import React from "react";
import {Button} from "primereact/button";
import ExplorePage from  "./Pages/ExplorePage"
import PersonalPage from "./Pages/PersonalPage";
import {Redirect, Route, Switch} from "react-router-dom";
import Post from "./Messages/Post";


class Home extends React.Component {
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
            explorePage: false,
            personalPage: true
        }
    }

    componentDidMount() {
        if (this.state.accountId !== null) {
            this.props.api.get('/user/' + this.props.accountId)
                .then(res => {
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
                })
        }
    }


    render() {
        return (
            <div className="App">
                <Header account={this.state.account} />
                <Switch>
                    <Route path={"/explore"}>
                        <ExplorePage api={this.props.api}></ExplorePage>
                    </Route>
                    <Route path={"/personal"}>
                        <PersonalPage api={this.props.api} visible={this.state.personalPage}></PersonalPage>
                    </Route>
                    <Route path={"/post/:id"}>
                        <Post></Post>
                    </Route>
                    <Route>
                        <Redirect to="/personal"/>
                    </Route>
                </Switch>
            </div>
        )
    }
}
export default Home