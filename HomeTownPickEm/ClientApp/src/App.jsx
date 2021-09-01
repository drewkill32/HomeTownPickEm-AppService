import React from 'react';
import './custom.css';

import { ProviderAuth } from './hooks/useAuth';
import { Route, Switch } from 'react-router';
import Layout from './components/Layout';
import Home from './components/Home';
import { QueryClient, QueryClientProvider } from 'react-query';
import { ReactQueryDevtools } from 'react-query/devtools';

import AuthorizeRoute from './components/AuthorizeRoute';

import Picks from './components/Picks';
import NotFound from './components/NotFound';
import Register from './components/auth/Register';
import Login from './components/auth/Login';
import Logout from './components/auth/Logout';
import Leaderboard from './pages/Leaderboard';
import WeeklyStats from './components/WeeklyStats';

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
            <Route path="/stats" component={WeeklyStats} />
            <AuthorizeRoute path="/leaderboard" component={Leaderboard} />
            <AuthorizeRoute path="/picks" component={Picks} />
            <Route path="*" component={NotFound} />
          </Switch>
        </Layout>
        <ReactQueryDevtools />
      </ProviderAuth>
    </QueryClientProvider>
  );
};

export default App;
