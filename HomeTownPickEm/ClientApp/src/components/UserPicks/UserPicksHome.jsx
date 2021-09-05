import React from 'react';
import {makeStyles} from '@material-ui/core/styles';
import {useQuery} from 'react-query';
import {useWeek} from '../../hooks/useWeek';
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
  const {isLoading, data} = useQuery(['userpick', week], () =>
      axios
          .get(`/api/picks/st-pete-pick-em/alluserpicks/${week}`)
          .then((res) => res.data)
  );
  if (isLoading) {
    return <UserPickSkeleton/>;
  }
  return (
      <div className={classes.root}>
        {data.map((pick, index) => (
            <UserPick
                key={pick.id || index}
                expanded={index === 0}
                pickCollection={pick}
            />
        ))}
      </div>
  );
}
