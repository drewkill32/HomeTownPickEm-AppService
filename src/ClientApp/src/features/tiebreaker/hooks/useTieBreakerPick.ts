import { useQuery, UseQueryOptions } from 'react-query';
import axios from 'axios';
import { RequestErrorType } from '../../../zod';

export const useTieBreakerPick = (
  pickId: string,
  options: Omit<
    UseQueryOptions<{ totalPoints: number }, RequestErrorType>,
    'queryFn' | 'queryKey'
  >,
) => {
  return useQuery<{ totalPoints: number }, RequestErrorType>({
    queryKey: ['tiebreaker-pick', pickId],
    queryFn: () =>
      axios.get(`/api/pick/tiebreaker-pick/${pickId}`).then((res) => res.data),
    ...options,
  });
};
