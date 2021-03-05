import Header from "./Header";
import React from "react";


class Home extends React.Component{
    constructor(props) {
        super(props);

        this.state = {
            loggedIn: false,
        }
    }

    render() {
        document.body.style.backgroundColor = "gray"
        return (
            <div className="App">
                <Header/>
                <body style={{backgroundColor: "gray", textAlign: "center"}}>
                    <p>
                        test
                    </p>
                </body>
            </div>
        )
    }
}
export default Home