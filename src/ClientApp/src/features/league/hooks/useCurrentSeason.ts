import axios from 'axios';
import { useLocalQuery } from '../../../hooks/useLocalQuery';

export interface CurrentSeasonProps {
  season: 'string';
  firstGameStart: Date;
  lastGameStart: Date;
}

export const useCurrentSeason = () => {
  return useLocalQuery<CurrentSeasonProps>(
    'currentSeason',
    () =>
      axios
        .get('/api/season/current')
        .then((res) => ({
          ...res.data,
          firstGameStart: new Date(res.data.firstGameStart),
          lastGameStart: new Date(res.data.lastGameStart),
        })),
    {
      staleTime: 1000 * 60 * 60 * 24,
    }
  );
};
