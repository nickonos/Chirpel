import React from "react"
import {Form, FormControl, Nav, Button, Navbar, NavDropdown} from "react-bootstrap";
import logo from './logo-rounded.png'

function Header() {
    return(
        <Navbar bg="dark" variant="dark" >
            <Navbar.Brand href="#home">
                <img src={logo}
                height="35"
                width="45"
                />
            </Navbar.Brand>

            <Nav className="mr-auto">
            </Nav>
            <Nav className="mr-md-auto">
                <Form inline>
                    <FormControl type="text" placeholder="Username" size="sm" />
                </Form>
            </Nav>
            <Nav>
                <Nav.Link href="#home">Home</Nav.Link>
                <Nav.Link href="#post">Post</Nav.Link>
                <Nav.Link href="#explore">Explore</Nav.Link>
                <NavDropdown title="Username" id="basic-nav-dropdown">
                    <NavDropdown.Item href="#action/3.1">Profile</NavDropdown.Item>
                    <NavDropdown.Item href="#action/3.2">Settings</NavDropdown.Item>
                    <NavDropdown.Item href="#action/3.3">Help</NavDropdown.Item>
                    <NavDropdown.Divider />
                    <NavDropdown.Item href="#action/3.4">Log Out</NavDropdown.Item>
                </NavDropdown>
            </Nav>
        </Navbar>
    )
}

export default Header