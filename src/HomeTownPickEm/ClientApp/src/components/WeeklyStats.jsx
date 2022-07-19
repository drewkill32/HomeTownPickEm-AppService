import useSchedule from '../hooks/useSchedule';
import { useWeek } from '../hooks/useWeek';
import Schedule from './Schedule';
import isAfter from 'date-fns/isAfter';
import Callout from './Callout';
import UserPicksHome from './UserPicks/UserPicksHome';

const WeeklyStats = () => {
  var week = useWeek();
  var { data, isLoading } = useSchedule(week);

  const isPastCutoff = () => {
    if (!data || data.length === 0) {
      return false;
    }
    return isAfter(new Date(), new Date(data[0].cutoffDate));
  };

  if (isLoading) {
    return (
      <div>
        <Schedule />
      </div>
    );
  }

  if (data) {
    return (
      <div>
        <Schedule />
        {isPastCutoff() ? <UserPicksHome /> : <Callout />}
      </div>
    );
  }
};

export default WeeklyStats;
