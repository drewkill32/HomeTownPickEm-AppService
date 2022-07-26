import axios from 'axios';
import { useMutation } from 'react-query';

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
  return useMutation((body: CreateSeasonParams) =>
    axios.post(`/api/league/new-season`, body).then((res) => res.data)
  );
};
