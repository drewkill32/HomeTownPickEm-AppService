import { Navigate, useLocation } from 'react-router-dom';
import { useAuth } from '../../authentication';
import { useLeague } from '../contexts/LeagueProvider';

export const LeagueSelection = () => {
  const [league, setLeague] = useLeague();
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
  }
  return <div>LeagueSelection</div>;
};
export default LeagueSelection;
