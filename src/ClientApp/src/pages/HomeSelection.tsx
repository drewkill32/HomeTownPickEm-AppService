import { Navigate } from 'react-router-dom';
import { useAuth } from '../features/authentication';
import { useLeague } from '../features/league';

const HomeSelection = () => {
  const { getUser } = useAuth();
  const { data: user } = getUser();
  const [league] = useLeague();

  if (!user) {
    return null;
  }
  if (league) {
    return <Navigate to={`/league/${league.slug}/${league.season}`} />;
  }
  return <Navigate to={`/home`} />;
};
export default HomeSelection;
