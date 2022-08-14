import { useMutation, useQueryClient } from 'react-query';
import { useLeague } from '../contexts/LeagueProvider';
import { leagueAgent } from '../utils/leagueAgent';
import { LeagueKeys } from '../utils/queryKeys';

export const useRemoveMember = () => {
  const queryClient = useQueryClient();
  const league = useLeague();

  if (!league) {
    throw new Error('Unable to remove member. League not found');
  }
  return useMutation(
    (memberId: string) =>
      leagueAgent.removeMember({
        leagueId: league.id,
        season: league.season,
        memberId,
      }),
    {
      onSuccess: () => queryClient.invalidateQueries(LeagueKeys.LeagueAdmin),
    }
  );
};
