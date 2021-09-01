import axios from 'axios';
import { useQuery } from 'react-query';

export default function useSchedule(week) {
  return useQuery(['schedule', week], () =>
    axios
      .get(`api/calendar/st-pete-pick-em?week=${week}`)
      .then((res) => res.data)
  );
}
