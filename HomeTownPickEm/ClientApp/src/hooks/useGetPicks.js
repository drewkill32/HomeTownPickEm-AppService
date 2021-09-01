import axios from 'axios';
import { useQuery } from 'react-query';
import { useAuth } from './useAuth';
import { useWeek } from './useWeek';

export default function useGetPicks() {
  const { user } = useAuth();
  const week = useWeek();
  const id = user.id;

  return useQuery(['picks', user.id, week], () =>
    axios
      .get(`api/picks/st-pete-pick-em/${id}/week/${week}`)
      .then((res) => res.data)
  );
}
