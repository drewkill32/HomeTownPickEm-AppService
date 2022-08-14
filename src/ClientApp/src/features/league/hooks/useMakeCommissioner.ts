import { useMutation, useQueryClient } from 'react-query';
import { useLeague } from '../contexts/LeagueProvider';
import { leagueAgent } from '../utils/leagueAgent';
import { LeagueKeys } from '../utils/queryKeys';

export const useMakeCommissioner = () => {
  const queryClient = useQueryClient();
  const league = useLeague();

  if (!league) {
    throw new Error('Unable to make member a commissioner. League not found');
  }
  return useMutation(
    (memberId: string) =>
      leagueAgent.makeCommissioner({
        leagueId: league.id,
        memberId,
      }),
    {
      onSuccess: () => queryClient.invalidateQueries(LeagueKeys.LeagueAdmin),
    }
  );
};
