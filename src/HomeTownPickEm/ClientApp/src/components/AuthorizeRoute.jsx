import React from "react";
import {Redirect, Route} from "react-router-dom";
import {useAuth} from "../hooks/useAuth";

const AuthorizeRoute = ({component, path, ...rest}) => {
    const {user} = useAuth();

    const Component = component;

    return (
        <Route
            {...rest}
            render={(props) => {
                if (user) {
                    return <Component {...props} />;
                } else {
                    return <Redirect to="/login"/>;
                }
            }}
        />
    );
};

export default AuthorizeRoute;
