import MainLayout from '../../../layout/MainLayout';
import { useAuth } from '../../authentication';
import { useLeague } from '../contexts/LeagueProvider';

const LeagueLayout = ({ children }: { children: JSX.Element }) => {
  const { user } = useAuth();
  const l = useLeague();
  const pages = [
    { name: 'Leagues', path: '/leagues' },
    { name: 'Leaderboard', path: `/league/${l.slug}/${l.season}` },
    { name: 'Picks', path: `/league/${l.slug}/${l.season}/weekly-stats` },
  ];
  if (user && l && user.claims['commissioner'] === l.id.toString()) {
    pages.push({ name: 'Admin', path: `/league/${l.slug}/${l.season}/admin` });
  }

  return (
    <MainLayout pages={pages} header={l.season}>
      {children}
    </MainLayout>
  );
};
export default LeagueLayout;
