import { useAuth } from '../features/authentication';
import { LeagueHome } from '../features/league';

const HomeSelection = () => {
  const { user } = useAuth();

  if (!user) {
    return null;
  }
  return <LeagueHome />;
};
export default HomeSelection;
