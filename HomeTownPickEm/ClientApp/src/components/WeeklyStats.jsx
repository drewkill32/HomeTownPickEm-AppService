import React from 'react';
import useSchedule from '../hooks/useSchedule';
import { useWeek } from '../hooks/useWeek';
import Schedule from './Schedule';
import isAfter from 'date-fns/isAfter';
import Callout from './Callout';

const WeeklyStats = () => {
  var week = useWeek();
  var { data, isLoading } = useSchedule(week);

  const isPastCutoff = () => {
    if (!data || data.length === 0) {
      return false;
    }
    return isAfter(new Date(), data[0].cutoffDate);
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
        {isPastCutoff() ? <div>TODO add picks</div> : <Callout />}
      </div>
    );
  }
};

export default WeeklyStats;
