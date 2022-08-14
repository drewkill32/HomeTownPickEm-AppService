import { useQuery } from 'react-query';
import { useLeague } from '../contexts/LeagueProvider';
import { leagueAgent } from '../utils/leagueAgent';
import { RequestErrorType } from '../../../zod';
import { LeagueAdminResult } from '../types';
import { LeagueKeys } from '../utils/queryKeys';

export const useLeagueAdmin = () => {
  const league = useLeague();
  return useQuery<LeagueAdminResult, RequestErrorType>(
    LeagueKeys.LeagueAdmin,
    () => leagueAgent.admin(league),
    {
      enabled: Boolean(league),
      staleTime: 1000 * 60,
    }
  );
};
