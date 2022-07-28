import { useParams } from 'react-router-dom';
import MainLayout from '../../../layout/MainLayout';
import { useAuth } from '../../authentication';
import { useLeague } from '../contexts/LeagueProvider';

const LeagueLayout = ({ children }: { children: JSX.Element }) => {
  const { league, season } = useParams();

  const { user } = useAuth();
  const [l] = useLeague();

  const pages = [
    { name: 'Leagues', path: '/' },
    { name: 'Leaderboard', path: `/league/${league}/${season}` },
    { name: 'Picks', path: `/league/${league}/${season}/weekly-stats` },
  ];
  console.log({ user, league: l });
  if (user && l && user.roles.includes(`commissioner:${l.id}`)) {
    pages.push({ name: 'Admin', path: `/league/${league}/${season}/admin` });
  }

  return (
    <MainLayout pages={pages} header={season}>
      {children}
    </MainLayout>
  );
};
export default LeagueLayout;
