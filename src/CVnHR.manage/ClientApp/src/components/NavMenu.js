import React from 'react';
import { Link } from 'react-router-dom';
import { Nav, Navbar, NavItem } from 'react-bootstrap';
import { LinkContainer } from 'react-router-bootstrap';
import './NavMenu.css';

const NavMenu = props =>
    <Navbar bg="dark" variant="dark" collapseOnSelect defaultExpanded fixed={'top'}>
        <Navbar.Brand>
            <Link to={'/'}>CVnHR.manage</Link>
        </Navbar.Brand>
        <Navbar.Toggle />
        <Navbar.Collapse>
            <Nav>
                <LinkContainer to={'/'} exact>
                    <NavItem>
                        Home
                    </NavItem>
                </LinkContainer>
                <LinkContainer to={'/counter'}>
                    <NavItem>
                        Counter
                    </NavItem>
                </LinkContainer>
                <LinkContainer to={'/fetchdata'}>
                    <NavItem>
                        Fetch data
                    </NavItem>
                </LinkContainer>
                <LinkContainer to={'/settings'}>
                    <NavItem>
                        Settings
                    </NavItem>
                </LinkContainer>
            </Nav>
        </Navbar.Collapse>
    </Navbar>;

export default NavMenu;
