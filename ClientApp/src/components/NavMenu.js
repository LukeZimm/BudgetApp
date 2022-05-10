import React, { Component } from 'react';
import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { useIsAuthenticated } from "@azure/msal-react";
import { Link } from 'react-router-dom';
import './NavMenu.css';
import { SignInButton } from './SignInButton';
import { SignOutButton } from './SignOutButton';


export const NavMenu = (props) => {
  const isAuthenticated = useIsAuthenticated();
  return (
    <header>
      <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" light>
        <Container>
          <NavbarBrand tag={Link} to="/">BudgetApp</NavbarBrand>
            <ul className="navbar-nav flex-grow">
              <NavItem>
                <NavLink tag={Link} className="text-dark" to="/">Home</NavLink>
              </NavItem>
              <NavItem>
                <NavLink tag={Link} className="text-dark" to="/counter">Counter</NavLink>
              </NavItem>
              <NavItem>
                <NavLink tag={Link} className="text-dark" to="/fetch-data">Fetch data</NavLink>
              </NavItem>
              <NavItem>
                {isAuthenticated ? <NavLink><SignOutButton/></NavLink> :
                  <NavLink><SignInButton/></NavLink>
                }
              </NavItem>
            </ul>
        </Container>
      </Navbar>
    </header>
  );
}
