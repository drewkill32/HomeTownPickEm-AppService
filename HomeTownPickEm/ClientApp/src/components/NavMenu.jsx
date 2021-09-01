import React, { useState } from 'react';
import {
  Collapse,
  Container,
  Navbar,
  NavbarBrand,
  NavbarToggler,
} from 'reactstrap';
import { Link } from 'react-router-dom';
import LoginMenu from './auth/LoginMenu';
import './NavMenu.css';

const NavMenu = () => {
  const [collapsed, setCollapsed] = useState(true);

  const toggleNavbar = () => {
    setCollapsed((c) => !c);
  };

  return (
    <header>
      <Navbar
        className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3"
        light
      >
        <Container>
          <NavbarBrand tag={Link} to="/">
            St. Pete Pick-Em
          </NavbarBrand>
          <NavbarToggler onClick={toggleNavbar} className="mr-2" />
          <Collapse
            className="d-sm-inline-flex flex-sm-row-reverse"
            isOpen={!collapsed}
            navbar
          >
            <ul className="navbar-nav flex-grow">
              <LoginMenu></LoginMenu>
            </ul>
          </Collapse>
        </Container>
      </Navbar>
    </header>
  );
};

export default NavMenu;
