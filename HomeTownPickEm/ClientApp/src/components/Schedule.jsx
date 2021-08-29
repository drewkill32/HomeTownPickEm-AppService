import React from 'react';
import {useQuery} from 'react-query';
import {useHistory} from 'react-router';
import {Button} from 'reactstrap';
import {useAuth} from '../hooks/useAuth';
import {useWeek} from '../hooks/useWeek';

const Schedule = () => {
  const {getToken} = useAuth();
  const history = useHistory();
  const week = useWeek();

  const {
    data: schedule,
    status,
    error,
  } = useQuery('schedule', () =>
      fetch('api/calendar', {
        headers: {Authorization: `Bearer ${getToken()}`},
      }).then((res) => res.json())
  );

  const handleClick = (direction) => {
    if (direction === 'next') {
      history.push({
        pathname: '/picks',
        search: `?week=${parseInt(week, 10) + 1}`,
      });
    } else {
      history.push({
        pathname: '/picks',
        search: `?week=${parseInt(week, 10) - 1}`,
      });
    }
  };

  if (status === 'loading') {
    return null;
  }
  if (status === 'success') {
    const maxWeek = Math.max(...schedule.map((s) => s.week));
    const currentWeek = schedule.find((s) => s.week === parseInt(week, 0)) || {
      week: week,
    };
    return (
        <div className="d-flex flex-row justify-content-between">
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
