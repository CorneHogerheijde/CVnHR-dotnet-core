import React from 'react';
import { Link } from 'react-router-dom';
import { Nav, Navbar } from 'react-bootstrap';
import { LinkContainer } from 'react-router-bootstrap';
import './NavMenu.css';

const NavMenu = props =>
    <Navbar collapseOnSelect bg="dark" variant="dark" expand="md" fixed={'top'}>
        <Navbar.Brand>
            <Link to={'/'} exact>CVnHR.manage</Link>
        </Navbar.Brand>
        <Navbar.Toggle aria-controls="responsive-navbar-nav" />
        <Navbar.Collapse id="responsive-navbar-nav">
            <Nav className="flex-column">
                <Nav.Item>
                    <LinkContainer to={'/'} exact>
                        <Nav.Link>
                            Home
                        </Nav.Link>
                    </LinkContainer>
                </Nav.Item>
                <Nav.Item>
                    <LinkContainer to={'/counter'}>
                        <Nav.Link>
                            Counter
                </Nav.Link>
                    </LinkContainer></Nav.Item>
                <Nav.Item><LinkContainer to={'/fetchdata'}>
                    <Nav.Link>
                        Fetch data
                </Nav.Link>
                </LinkContainer></Nav.Item>
                <Nav.Item><LinkContainer to={'/settings'}>
                    <Nav.Link>
                        Settings
                </Nav.Link>
                </LinkContainer></Nav.Item>
            </Nav>
        </Navbar.Collapse >
    </Navbar>;

export default NavMenu;
