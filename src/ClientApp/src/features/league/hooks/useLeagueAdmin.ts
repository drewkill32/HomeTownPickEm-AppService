import axios from 'axios';
import { useQuery } from 'react-query';
import { useLeague } from '../contexts/LeagueProvider';

//
export interface LeagueAdminResult {
  members: LeagueAdminMember[];
  teams: LeagueAdminTeam[];
}

export interface LeagueAdminMember {
  firstName: string;
  lastName: string;
  email: string;
  id: string;
  profileImg: string;
  fullName: string;
  initials: string;
  color: string;
}

export interface LeagueAdminTeam {
  color: string;
  altColor: string;
  logo: string;
  school: string;
  mascot: string;
  name: string;
  id: number;
}

export const useLeagueAdmin = () => {
  const [league] = useLeague();
  return useQuery<LeagueAdminResult>(
    'league-admin',
    () =>
      axios
        .get(`/api/League/${league?.id}/${league?.season}/MembersTeams`)
        .then((res) => res.data),
    {
      enabled: Boolean(league),
      staleTime: 1000 * 60,
    }
  );
};
