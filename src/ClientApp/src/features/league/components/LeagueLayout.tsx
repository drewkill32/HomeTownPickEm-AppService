import { useParams } from 'react-router-dom';
import MainLayout from '../../../layout/MainLayout';

const LeagueLayout = ({ children }: { children: JSX.Element }) => {
  const { league, season } = useParams();

  const pages = [
    { name: 'Leagues', path: '/home' },
    { name: 'Leaderboard', path: `/league/${league}/${season}` },
    { name: 'Picks', path: `/league/${league}/${season}/weekly-picks` },
  ];

  return <MainLayout pages={pages}>{children}</MainLayout>;
};
export default LeagueLayout;
