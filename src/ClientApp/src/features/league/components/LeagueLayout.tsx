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
  if (user && l && user.claims['commissioner'] === l.id.toString()) {
    pages.push({ name: 'Admin', path: `/league/${league}/${season}/admin` });
  }

  return (
    <MainLayout pages={pages} header={season}>
      {children}
    </MainLayout>
  );
};
export default LeagueLayout;
