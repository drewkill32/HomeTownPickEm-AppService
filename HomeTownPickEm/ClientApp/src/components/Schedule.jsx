import axios from 'axios';
import React from 'react';
import {useQuery} from 'react-query';
import {useHistory} from 'react-router';
import {useLocation} from 'react-router-dom';
import {Button} from 'reactstrap';
import {useWeek} from '../hooks/useWeek';

const Schedule = () => {
  const history = useHistory();
  const {pathname} = useLocation();
  const week = useWeek();

  const {
    data: schedule,
    status,
    error,
  } = useQuery('schedule', () =>
    axios.get('api/calendar/st-pete-pick-em').then((res) => res.data)
  );

  const handleClick = (direction) => {
    if (direction === 'next') {
      history.push({
        pathname: pathname,
        search: `?week=${parseInt(week, 10) + 1}`,
      });
    } else {
      history.push({
        pathname: pathname,
        search: `?week=${parseInt(week, 10) - 1}`,
      });
    }
  };

  if (status === 'loading') {
    return null;
  }
  if (status === 'success') {
    const maxWeek = Math.max(...schedule.map((s) => s.week));
    const currentWeek = schedule.find((s) => s.week === parseInt(week, 10)) || {
      week: week,
    };
    return (
        <div className="d-flex flex-row justify-content-between mb-4">
          <Button
              disabled={status === 'loading' || week <= 1}
              onClick={() => handleClick('prev')}
              color="primary"
          >
            {'<'}
          </Button>
          <h1>{currentWeek.week}</h1>
          <Button
              disabled={status === 'loading' || week >= maxWeek}
              onClick={() => handleClick('next')}
              color="primary"
          >
            {'>'}
          </Button>
        </div>
    );
  }
  console.error(error);
  return null;
};

export default Schedule;
