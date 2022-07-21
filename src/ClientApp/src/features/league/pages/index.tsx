import { Outlet } from 'react-router-dom';
import LeagueLayout from '../components/LeagueLayout';

function LeagueIndex() {
  return (
    <LeagueLayout>
      <Outlet />
    </LeagueLayout>
  );
}
export default LeagueIndex;
