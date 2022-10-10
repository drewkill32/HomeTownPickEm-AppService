import MainLayout from '../../../layout/MainLayout';
import { useAuth } from '../../authentication';
import { useLeague } from '../contexts/LeagueProvider';
import { useCurrentSeason } from '../hooks/useCurrentSeason';
import { useEffect, useState } from 'react';

const LeagueLayout = ({ children }: { children: JSX.Element }) => {
  const [week, setWeek] = useState(1);
  const { user } = useAuth();
  const l = useLeague();
  const query = useCurrentSeason();

  useEffect(() => {
    setWeek(query.data?.week || 1);
  }, [query.data]);

  const pages = [
    { name: 'Leagues', path: '/leagues' },
    { name: 'Leaderboard', path: `/league/${l.slug}/${l.season}` },
    {
      name: 'Picks',
      path: `/league/${l.slug}/${l.season}/weekly-stats?week=${week}`,
    },
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
