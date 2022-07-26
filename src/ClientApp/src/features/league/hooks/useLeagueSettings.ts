import axios from 'axios';
import { useQuery } from 'react-query';

export interface LeagueSettingsProps {
  id: number;
  name: string;
  slug: string;
  imageUrl: string;
  years: [LeagueSettingsYear];
}

export interface LeagueSettingsYear {
  year: string;
  teamCount: number;
  memberCount: number;
}

export const useLeagueSettings = (leagueId: number) => {
  return useQuery<LeagueSettingsProps>(
    ['league', leagueId],
    () => axios.get(`/api/league/settings/${leagueId}`).then((res) => res.data),
    {
      enabled: Boolean(leagueId),
    }
  );
};
