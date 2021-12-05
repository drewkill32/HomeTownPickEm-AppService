import React from 'react';
import './custom.css';

import {ProviderAuth} from './hooks/useAuth';
import {Route, Switch} from 'react-router';
import Layout from './components/Layout';
import Home from './components/Home';
import {QueryClient, QueryClientProvider} from 'react-query';
import {ReactQueryDevtools} from 'react-query/devtools';

import AuthorizeRoute from './components/AuthorizeRoute';

import NotFound from './components/NotFound';
import Register from './components/auth/Register';
import Login from './components/auth/Login';
import Logout from './components/auth/Logout';
import Leaderboard from './pages/Leaderboard';
import WeeklyStats from './components/WeeklyStats';
import PicksHome from './components/picks/PicksHome';
import Unauthorized from './pages/Unauthorized';

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
              <AuthorizeRoute path="/picks" component={PicksHome}/>
              <Route path="/unauthorized" component={Unauthorized}/>
              <Route path="*" component={NotFound}/>
          </Switch>
        </Layout>
        <ReactQueryDevtools />
      </ProviderAuth>
    </QueryClientProvider>
  );
};

export default App;
