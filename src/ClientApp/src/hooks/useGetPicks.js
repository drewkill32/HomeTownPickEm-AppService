import axios from 'axios';
import { useQuery } from 'react-query';
import { useAuth } from '../features/authentication';
import { useWeek } from './useWeek';

export default function useGetPicks() {
  const {
    user: { id },
  } = useAuth();
  const week = useWeek();

  return useQuery(['picks', id, week], () =>
    axios.get(`api/picks/st-pete-pick-em/${id}/week/${week}`).then((res) =>
      res.data.map((g) => ({
        ...g,
        cutoffDate: new Date(g.cutoffDate),
        startDate: new Date(g.startDate),
      }))
    )
  );
}
