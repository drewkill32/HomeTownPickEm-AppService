import React, {Fragment, useEffect, useState} from "react";
import {NavItem, NavLink} from "reactstrap";
import {Link} from "react-router-dom";
import {useAuth} from "../../hooks/useAuth";

const AuthenticatedView = ({userName}) => {
    return (
        <Fragment>
            <NavItem>
                <NavLink tag={Link} className="text-dark" to="/profile">
                    Hello {userName}
                </NavLink>
            </NavItem>
            <NavItem>
                <NavLink tag={Link} className="text-dark" to="logout">
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
    const {user} = useAuth();
    const [userName, setUserName] = useState("");

    useEffect(() => {
        console.log({user});
        setUserName(user && user.email);
    }, [user]);

    if (!user) {
        return <AnonymousView/>;
    } else {
        return (
            <AuthenticatedView
                userName={userName}
                profilePath="/profile"
                logoutPath="/logout"
            />
        );
    }
};

export default LoginMenu;
