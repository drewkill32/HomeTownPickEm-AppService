import React, { Fragment } from 'react';
import { NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import { useAuth } from '../../hooks/useAuth';

const AuthenticatedView = ({ user }) => {
  return (
    <Fragment>
      <NavItem>
        <NavLink tag={Link} className="text-dark" to="/">
          <span>
            <img
              src={user.team.logo}
              alt="user"
              style={{ marginRight: '0.5rem' }}
              width="25"
              height="25"
            />
            {user.firstName} {user.lastName}
          </span>
        </NavLink>
      </NavItem>
      <NavItem>
        <NavLink tag={Link} className="text-dark" to="/logout">
          Logout
        </NavLink>
      </NavItem>
    </Fragment>
  );
};

const AnonymousView = () => {
  return (
    <Fragment>
      <NavItem>
        <NavLink tag={Link} className="text-dark" to="/register">
          Register
        </NavLink>
      </NavItem>
      <NavItem>
        <NavLink tag={Link} className="text-dark" to="/login">
          Login
        </NavLink>
      </NavItem>
    </Fragment>
  );
};

const LoginMenu = () => {
  const { user } = useAuth();

  if (!user) {
    return <AnonymousView />;
  } else {
    return (
      <AuthenticatedView
        user={user}
        profilePath="/profile"
        logoutPath="/logout"
      />
    );
  }
};

export default LoginMenu;
