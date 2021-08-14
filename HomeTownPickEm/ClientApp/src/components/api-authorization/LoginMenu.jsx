import React, { useState, useEffect, Fragment } from "react";
import { NavItem, NavLink } from "reactstrap";
import { Link } from "react-router-dom";
import authService from "./AuthorizeService";
import { ApplicationPaths } from "./ApiAuthorizationConstants";

const AuthenticatedView = ({ userName, profilePath, logoutPath }) => {
  return (
    <Fragment>
      <NavItem>
        <NavLink tag={Link} className="text-dark" to={profilePath}>
          Hello {userName}
        </NavLink>
      </NavItem>
      <NavItem>
        <NavLink tag={Link} className="text-dark" to={logoutPath}>
          Logout
        </NavLink>
      </NavItem>
    </Fragment>
  );
};

const AnonymousView = ({ registerPath, loginPath }) => {
  return (
    <Fragment>
      <NavItem>
        <NavLink tag={Link} className="text-dark" to={registerPath}>
          Register
        </NavLink>
      </NavItem>
      <NavItem>
        <NavLink tag={Link} className="text-dark" to={loginPath}>
          Login
        </NavLink>
      </NavItem>
    </Fragment>
  );
};

const LoginMenu = () => {
  const [authenticated, setAuthenticated] = useState(false);
  const [userName, setUserName] = useState("");

  useEffect(() => {
    const subscription = authService.subscribe(() => populateState());
    populateState();
    return () => subscription.unsubscribe();
  }, []);

  const populateState = async () => {
    const [isAuthenticated, user] = await Promise.all([
      authService.isAuthenticated(),
      authService.getUser(),
    ]);
    setAuthenticated(isAuthenticated);
    setUserName(user && user.name);
  };

  if (!authenticated) {
    const registerPath = `${ApplicationPaths.Register}`;
    const loginPath = `${ApplicationPaths.Login}`;
    return <AnonymousView registerPath={registerPath} loginPath={loginPath} />;
  } else {
    const profilePath = `${ApplicationPaths.Profile}`;
    const logoutPath = {
      pathname: `${ApplicationPaths.LogOut}`,
      state: { local: true },
    };
    return (
      <AuthenticatedView
        userName={userName}
        profilePath={profilePath}
        logoutPath={logoutPath}
      />
    );
  }
};

export default LoginMenu;
