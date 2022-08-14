import { Outlet } from 'react-router-dom';
import LeagueLayout from '../components/LeagueLayout';
import { LeagueProvider } from '../contexts/LeagueProvider';

function LeagueIndex() {
  return (
    <LeagueProvider>
      <LeagueLayout>
        <Outlet />
      </LeagueLayout>
    </LeagueProvider>
  );
}
export default LeagueIndex;
