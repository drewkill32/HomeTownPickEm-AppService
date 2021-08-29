import React from 'react';
import './custom.css';

import { ProviderAuth } from './hooks/useAuth';
import { Route, Switch } from 'react-router';
import Layout from './components/Layout';
import Home from './components/Home';
import { QueryClient, QueryClientProvider } from 'react-query';

import AuthorizeRoute from './components/AuthorizeRoute';

import Picks from './components/Picks';
import NotFound from './components/NotFound';
import Register from './components/Login/Register';
import Login from './components/Login/Login';
import Logout from './components/Login/Logout';
import Leaderboard from './pages/Leaderboard';

const queryClient = new QueryClient();

const App = () => {
  return (
    <QueryClientProvider client={queryClient}>
      <ProviderAuth>
        <Layout>
          <Switch>
            <Route exact path="/" component={Home} />
            <Route path="/register" component={Register} />
            <Route path="/login" component={Login} />
            <Route path="/logout" component={Logout} />
            <AuthorizeRoute path="/leaderboard" component={Leaderboard} />
            <AuthorizeRoute path="/picks" component={Picks} />
            <Route path="*" component={NotFound} />
          </Switch>
        </Layout>
      </ProviderAuth>
    </QueryClientProvider>
  );
};

export default App;
