import React from "react"
import {Form, FormControl, Nav, Navbar, NavDropdown} from "react-bootstrap";
import logo from './logo-rounded.png'
import {BrowserRouter as Router, Link} from "react-router-dom";

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
                <Nav className="mr-md-auto">
                    <Form inline>
                        <FormControl type="text" placeholder="Username" size="sm" />
                    </Form>
                </Nav>
                <Nav>
                    <Link to={'/home'}>
                        <Nav.Link href="#home">Home</Nav.Link>
                    </Link>
                    <Link to={'/post'}>
                        <Nav.Link href="#post">Post</Nav.Link>
                    </Link>
                    <Link to={'/explore'}>
                        <Nav.Link href="#explore">Explore</Nav.Link>
                    </Link>
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