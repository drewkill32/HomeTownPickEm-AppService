import { Navigate, useLocation } from 'react-router-dom';
import { useAuth } from '../../authentication';
import { useLeague } from '../contexts/LeagueProvider';

export const LeagueSelection = () => {
  let location = useLocation();
  const { getUser, isAuthenticated } = useAuth();
  const { data: user } = getUser();
  if (!isAuthenticated) {
    return (
      <Navigate
        to="/login"
        state={{
          from: location,
        }}
      />
    );
  }
  if (!user) {
    return <div>Geting user</div>;
  }
  if (user.leagues.length === 1 && user.leagues[0].years.length === 1) {
    const league = user.leagues[0];
    const season = league.years[0];
    return <Navigate to={`/league/${league.slug}/${season}`} />;
  }
  return <div>Under Construction Select league</div>;
};
export default LeagueSelection;
