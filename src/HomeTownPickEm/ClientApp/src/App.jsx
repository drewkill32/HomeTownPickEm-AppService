import { AuthProvider } from './features/authentication';
import { Route, Routes } from 'react-router';
import Home from './components/Home';
import { QueryClient, QueryClientProvider } from 'react-query';
import { ReactQueryDevtools } from 'react-query/devtools';

import RequireAuth from './components/AuthorizeRoute';

import NotFound from './components/NotFound';
import Register from './components/auth/Register';
import Login from './components/auth/Login';
import Logout from './components/auth/Logout';
import { LeagueProvider, LeagueSelection } from './features/league';

const queryClient = new QueryClient();

const App = () => {
  return (
    <QueryClientProvider client={queryClient}>
      <AuthProvider>
        <LeagueProvider>
          <Routes>
            <Route
              path="/"
              element={
                <RequireAuth>
                  <Home />
                </RequireAuth>
              }
            >
              <Route index element={<LeagueSelection />}></Route>
            </Route>
            <Route path="/register" element={<Register />} />
            <Route path="/login" element={<Login />} />
            <Route
              path="/logout"
              element={
                <RequireAuth>
                  <Logout />
                </RequireAuth>
              }
            />

            <Route path="*" component={NotFound} />
          </Routes>
          <ReactQueryDevtools />
        </LeagueProvider>
      </AuthProvider>
    </QueryClientProvider>
  );
};

export default App;
