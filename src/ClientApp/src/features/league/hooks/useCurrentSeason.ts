import axios from 'axios';
import { useLocalQuery } from '../../../hooks/useLocalQuery';

export interface CurrentSeasonProps {
  season: 'string';
  firstGameStart: Date;
  lastGameStart: Date;
  week: number;
  weekStart: Date;
  weekEnd: Date;
}

export const useCurrentSeason = () => {
  return useLocalQuery<CurrentSeasonProps>(
    'currentSeason',
    () =>
      axios.get('/api/season/current').then((res) => ({
        ...res.data,
        firstGameStart: new Date(res.data.firstGameStart),
        lastGameStart: new Date(res.data.lastGameStart),
        weekStart: new Date(res.data.weekStart),
        weekEnd: new Date(res.data.weekEnd),
      })),
    {
      staleTime: 1000 * 60 * 60 * 24,
    }
  );
};
