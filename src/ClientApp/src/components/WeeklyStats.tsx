import Schedule from './Schedule';
import { useWeek } from '../features/SeasonPicks/hooks/useWeek';
import { useSchedule } from '../hooks/useSchedule';
import PicksHome from './picks/PicksHome';

const WeeklyStats = () => {
  const { week } = useWeek();
  const { data } = useSchedule();

  return (
    <div>
      <Schedule />
      <PicksHome />
    </div>
  );
  // if (isLoading) {
  //
  // }
  //
  // if (data) {
  //   return (
  //     <div>
  //       <Schedule />
  //       {isPastCutoff() ? <WeeklyPicks /> : <Callout />}
  //     </div>
  //   );
  // }
};

export default WeeklyStats;
