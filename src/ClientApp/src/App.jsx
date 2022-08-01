import {
  AuthProvider,
  ConfirmPasswordReset,
  ForgotPassword,
  Login,
  Register,
} from './features/authentication';
import { Route, Routes } from 'react-router-dom';
import Home from './pages/Home';
import { QueryClient, QueryClientProvider } from 'react-query';
import { ReactQueryDevtools } from 'react-query/devtools';

import NotFound from './components/NotFound';

import {
  Leaderboard,
  LeagueIndex,
  LeagueProvider,
  LeagueHome,
  LeagueAdmin,
} from './features/league';
import WeeklyStats from './components/WeeklyStats';
import { ProfilePage } from './features/profile';
import { PickProvider } from './features/SeasonPicks/contexts/PickContext';

const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      staleTime: 1000 * 60,
    },
  },
});

const App = () => {
  return (
    <QueryClientProvider client={queryClient}>
      <AuthProvider>
        <LeagueProvider>
          <Routes>
            <Route path="/" element={<Home />}>
              <Route index element={<LeagueHome />} />
              <Route path="profile" element={<ProfilePage />} />
              <Route path="league/:league/:season" element={<LeagueIndex />}>
                <Route index element={<Leaderboard />}></Route>
                <Route
                  path="weekly-stats"
                  element={
                    <PickProvider>
                      <WeeklyStats />
                    </PickProvider>
                  }
                />
                <Route path="admin" element={<LeagueAdmin />} />
              </Route>
            </Route>
            <Route path="/new-user" element={<Register />} />
            <Route path="/login" element={<Login />} />
            <Route path="/forgot-password" element={<ForgotPassword />} />
            <Route
              path="/confirm-reset-password"
              element={<ConfirmPasswordReset />}
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
