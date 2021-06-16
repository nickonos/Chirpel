import Header from "../Header";
import React from "react";
import ExplorePage from "./ExplorePage"
import PersonalPage from "./PersonalPage";
import {Route} from "react-router-dom";
import Post from "../Messages/Post";
import NavMenu from "../NavMenu";
import ProfilePage from "./ProfilePage";
import SettingsPage from "./SettingsPage";
import HelpPage from "./HelpPage"

class HomePage extends React.Component {
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
            explorePage: true,
            personalPage: false,
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
                <Header account={this.state.account} loggedin={this.props.loggedin} />

                {this.props.loggedin ? <NavMenu accountId={this.props.accountId} /> : ""}
                    <Route path={"/explore"} exact>
                         <ExplorePage api={this.props.api}/>
                    </Route>
                    <Route path={"/settings"} exact>
                        <SettingsPage api={this.props.api}/>
                    </Route>
                    <Route path={"/help"} exact>
                        <HelpPage/>
                    </Route>
                    <Route path={"/personal"} exact>
                        <PersonalPage api={this.props.api} visible={this.state.personalPage}/>
                    </Route>
                    <Route path={"/post/:id"} render={(props) =>
                        <Post id={props.match.params.id} api={this.props.api}/>
                    }/>
                    <Route path={"/profile/:id"} render={(props) =>
                        <ProfilePage id={props.match.params.id} accountId={this.props.accountId} api={this.props.api}/>
                    }/>

            </div>
        )
    }
}
export default HomePage