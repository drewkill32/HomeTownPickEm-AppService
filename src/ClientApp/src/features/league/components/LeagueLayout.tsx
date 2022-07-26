import { useParams } from 'react-router-dom';
import MainLayout from '../../../layout/MainLayout';

const LeagueLayout = ({ children }: { children: JSX.Element }) => {
  const { league, season } = useParams();

  const pages = [
    { name: 'Leagues', path: '/' },
    { name: 'Leaderboard', path: `/league/${league}/${season}` },
    { name: 'Picks', path: `/league/${league}/${season}/weekly-stats` },
  ];

  return (
    <MainLayout pages={pages} header={season}>
      {children}
    </MainLayout>
  );
};
export default LeagueLayout;
