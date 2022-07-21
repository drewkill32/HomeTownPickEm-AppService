import { Navigate, useLocation } from 'react-router-dom';
import { useAuth } from '../../authentication';
import { useLeague } from '../contexts/LeagueProvider';

export const LeagueSelection = () => {
  let location = useLocation();
  const { user } = useAuth();
  if (!user) {
    return (
      <Navigate
        to="/login"
        state={{
          from: location,
        }}
      />
    );
  }
  if (user.leagues.length === 1) {
    const [league, season] = user.leagues[0].split(':');
    return <Navigate to={`/league/${league}/${season}`} />;
  }
  return <div>Under Construction Select league</div>;
};
export default LeagueSelection;
