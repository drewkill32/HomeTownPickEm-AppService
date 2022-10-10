import { useAuth } from '../features/authentication';
import { LeagueHome } from '../features/league';
import { useGetUserLeagues, useCurrentSeason } from '../features/league';
import { Navigate } from 'react-router-dom';

const HomeSelection = () => {
  const { user } = useAuth();
  const { data: leagues } = useGetUserLeagues();
  const { data: season } = useCurrentSeason();

  if (!user || !leagues || !season) {
    return null;
  }
  console.log({ user, leagues });
  if (leagues.length === 1) {
    const l = leagues[0];
    const year = Math.max(...l.years.map((x) => Number(x)));
    return (
      <Navigate
        to={`/league/${l.slug}/${year}/weekly-stats?week=${season.week}`}
      />
    );
  }
  return <LeagueHome />;
};
export default HomeSelection;
