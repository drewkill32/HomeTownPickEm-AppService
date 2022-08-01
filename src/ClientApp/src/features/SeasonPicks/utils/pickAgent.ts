import axios, { AxiosResponse } from 'axios';
import { League } from '../../league/types';

const weeklyPicks = async (leagueId: number, season: string, week: number) => {
  axios
    .get(`/api/picks/league/${leagueId}/season/${season}/week/${week}`)
    .then((res) => res.data);
};

const weeklyUserPicks = (league: League, week: number) => {
  return axios
    .get(
      `/api/picks/league/${league.id}/season/${league.season}/week/${week}/user`
    )
    .then((res: AxiosResponse<any>) =>
      res.data.map((g: any) => ({
        ...g,
        cutoffDate: new Date(g.cutoffDate),
        startDate: new Date(g.startDate),
      }))
    );
};

export const pickAgent = {
  weeklyPicks,
  weeklyUserPicks,
};
