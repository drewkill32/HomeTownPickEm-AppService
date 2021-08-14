import React, { useState, useEffect } from "react";
import { Route, Redirect } from "react-router-dom";
import {
  ApplicationPaths,
  QueryParameterNames,
} from "./ApiAuthorizationConstants";
import authService from "./AuthorizeService";

const AuthorizeRoute = ({ component, path, ...rest }) => {
  const [ready, setReady] = useState(false);
  const [authenticated, setAuthenticated] = useState(false);
  const Component = component;
  const populateAuthenticationState = async () => {
    const authenticated = await authService.isAuthenticated();
    setAuthenticated(authenticated);
    setReady(true);
  };
  const authenticationChanged = async () => {
    setReady(false);
    setAuthenticated(false);
    await populateAuthenticationState();
  };

  useEffect(() => {
    const subscription = authService.subscribe(() => authenticationChanged());
    populateAuthenticationState();
    return () => authService.unsubscribe(subscription);
  }, []);

  var link = document.createElement("a");
  link.href = path;
  const returnUrl = `${link.protocol}//${link.host}${link.pathname}${link.search}${link.hash}`;
  const redirectUrl = `${ApplicationPaths.Login}?${
    QueryParameterNames.ReturnUrl
  }=${encodeURIComponent(returnUrl)}`;
  if (!ready) {
    return <div></div>;
  } else {
    return (
      <Route
        {...rest}
        render={(props) => {
          if (authenticated) {
            return <Component {...props} />;
          } else {
            return <Redirect to={redirectUrl} />;
          }
        }}
      />
    );
  }
};

export default AuthorizeRoute;
