import React from 'react';
import { Col, Container, Row } from 'react-bootstrap';
import NavMenu from './NavMenu';

const Layout = props =>
    <Container fluid>
        <Row>
            <Col sm={3}>
                <NavMenu />
            </Col>
            <Col sm={9}>
                {props.children}
            </Col>
        </Row>
    </Container>;

export default Layout;