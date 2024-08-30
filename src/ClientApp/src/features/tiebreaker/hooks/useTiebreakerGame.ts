import { useQuery } from 'react-query';
import axios from 'axios';
import { RequestErrorType } from '../../../zod';
import { TiebreakerGame } from '../../../games';

export const useTiebreakerGame = (body: { seasonId: number; week: number }) => {
  return useQuery<TiebreakerGame, RequestErrorType>({
    queryKey: ['tiebreaker-game', body],
    queryFn: () =>
      axios
        .get(`/api/League/${body.seasonId}/week/${body.week}/tiebreaker-game`)
        .then((res) => res.data),
  });
};
