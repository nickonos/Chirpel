import React from "react"
import {Form, FormControl, Nav, Navbar, NavDropdown} from "react-bootstrap";
import logo from './logo-rounded.png'
import {BrowserRouter as Router} from "react-router-dom";


function Header() {
    return(
        <Router>
            <Navbar bg="dark" variant="dark" >
                <Navbar.Brand href="#home">
                    <img src={logo}
                         height="35"
                         width="45"
                         alt="logo"
                    />
                </Navbar.Brand>
                <Nav className="mx-auto" >
                    <Form inline>
                        <FormControl type="text" placeholder="Username" size="md" />
                    </Form>
                </Nav>
                <Nav>
                    <Nav.Link href="/home">Home</Nav.Link>
                    <Nav.Link href="/post">Post</Nav.Link>
                    <Nav.Link href="/explore">Explore</Nav.Link>
                    <NavDropdown title="Username" id="basic-nav-dropdown">
                        <NavDropdown.Item href="{Username}">Profile</NavDropdown.Item>
                        <NavDropdown.Item href="{Username}/settings">Settings</NavDropdown.Item>
                        <NavDropdown.Item href="Help">Help</NavDropdown.Item>
                        <NavDropdown.Divider />
                        <NavDropdown.Item href="{Username/logout}">Log Out</NavDropdown.Item>
                    </NavDropdown>
                </Nav>
            </Navbar>
        </Router>
    )
}

export default Header