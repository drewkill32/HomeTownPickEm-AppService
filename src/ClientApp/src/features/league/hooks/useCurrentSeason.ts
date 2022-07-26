import axios from 'axios';
import { useLocalQuery } from '../../../hooks/useLocalQuery';

export interface CurrentSeasonProps {
  season: 'string';
}

export const useCurrentSeason = () => {
  return useLocalQuery<CurrentSeasonProps>(
    'currentSeason',
    () => axios.get('/api/season/current').then((res) => res.data),
    {
      staleTime: 1000 * 60 * 60 * 24,
    }
  );
};
