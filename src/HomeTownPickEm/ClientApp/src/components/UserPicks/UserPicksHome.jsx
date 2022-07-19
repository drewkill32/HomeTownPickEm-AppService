import React from 'react';
import { makeStyles } from '@mui/styles';
import { useQuery } from 'react-query';
import { useWeek } from '../../hooks/useWeek';
import axios from 'axios';
import UserPick from './UserPick';

import UserPickSkeleton from './UserPickSkeleton';

const useStyles = makeStyles((theme) => ({
  root: {
    width: '100%',
    minWidth: '200px',
  },
}));

export default function UserPicksHome() {
  const classes = useStyles();
  const week = useWeek();
  const { isLoading, data } = useQuery(['userpick', week], () =>
    axios
      .get(`/api/picks/st-pete-pick-em/alluserpicks/${week}`)
      .then((res) => res.data)
  );
  if (isLoading) {
    return <UserPickSkeleton />;
  }

  if (data) {
    return (
      <div className={classes.root}>
        {data.map((pick) => (
          <UserPick key={pick.user.id} pickCollection={pick} />
        ))}
      </div>
    );
  }
  return null;
}
