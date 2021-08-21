import React from "react";
import "./custom.css";

import {Route, Switch} from "react-router";
import Layout from "./components/Layout";
import Home from "./components/Home";
import FetchData from "./components/FetchData";
import Counter from "./components/Counter";
import AuthorizeRoute from "./components/api-authorization/AuthorizeRoute";
import ApiAuthorizationRoutes from "./components/api-authorization/ApiAuthorizationRoutes";
import {ApplicationPaths} from "./components/api-authorization/ApiAuthorizationConstants";

import Teams from "./components/Teams";
import Picks from "./components/Picks";
import NotFound from "./components/NotFound";

const App = () => {
  return (
      <Layout>
        <Switch>
          <Route exact path="/" component={Home}/>
          <Route path="/counter" component={Counter}/>
          <AuthorizeRoute path="/fetch-data" component={FetchData}/>
          <AuthorizeRoute path="/teams" component={Teams}/>
          <AuthorizeRoute path="/picks/:week" component={Picks}/>
          <Route
              path={ApplicationPaths.ApiAuthorizationPrefix}
              component={ApiAuthorizationRoutes}
          />
          <Route path="*" component={NotFound}/>
      </Switch>
    </Layout>
  );
};

export default App;
