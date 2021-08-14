import React from "react";
import { Route } from "react-router";
import Layout from "./components/Layout";
import Home from "./components/Home";
import FetchData from "./components/FetchData";
import Counter from "./components/Counter";
import AuthorizeRoute from "./components/api-authorization/AuthorizeRoute";
import ApiAuthorizationRoutes from "./components/api-authorization/ApiAuthorizationRoutes";
import { ApplicationPaths } from "./components/api-authorization/ApiAuthorizationConstants";

import "./custom.css";

const App = () => {
  return (
    <Layout>
      <Route exact path="/" component={Home} />
      <Route path="/counter" component={Counter} />
      <AuthorizeRoute path="/fetch-data" component={FetchData} />
      <Route
        path={ApplicationPaths.ApiAuthorizationPrefix}
        component={ApiAuthorizationRoutes}
      />
    </Layout>
  );
};

export default App;
