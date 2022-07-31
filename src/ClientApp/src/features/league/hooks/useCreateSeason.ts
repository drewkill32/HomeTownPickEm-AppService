import axios from 'axios';
import { useMutation, useQueryClient } from 'react-query';
import { LeagueKeys } from '../utils/queryKeys';

export interface CreateSeasonParams {
  leagueId: number;
  year: string;
  copyFrom?: {
    season: string;
    teams: boolean;
    members: boolean;
  };
}

export const useCreateSeason = () => {
  const queryClient = useQueryClient();
  return useMutation(
    (body: CreateSeasonParams) =>
      axios.post(`/api/league/new-season`, body).then((res) => res.data),
    {
      onSuccess: () => queryClient.invalidateQueries(LeagueKeys.UserLeagues),
    }
  );
};
