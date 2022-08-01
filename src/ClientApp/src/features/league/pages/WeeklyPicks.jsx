import { makeStyles } from '@mui/styles';
import { useQuery } from 'react-query';
import { useWeek } from '../../SeasonPicks/hooks/useWeek';
import axios from 'axios';
import UserPick from '../../../components/UserPicks/UserPick';

import UserPickSkeleton from '../../../components/UserPicks/UserPickSkeleton';
import { PickQueryKeys } from '../../SeasonPicks/utils/queryKeys';

const useStyles = makeStyles(() => ({
  root: {
    width: '100%',
    minWidth: '200px',
  },
}));

export function WeeklyPicks() {
  const classes = useStyles();
  const { week } = useWeek();
  const { isLoading, data } = useQuery([PickQueryKeys.UserPick, week], () =>
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
