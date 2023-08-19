import { useMutation, useQueryClient } from 'react-query';
import { useLeague } from '../contexts/LeagueProvider';
import { leagueAgent } from '../utils/leagueAgent';

export const useRemoveLeague = () => {
  const queryClient = useQueryClient();
  const league = useLeague();

  if (!league) {
    throw new Error('Unable to remove league. League not found');
  }
  return useMutation(
    () =>
      leagueAgent.removeLeague({
        leagueId: league.id,
        season: league.season,
      }),
    {
      onSuccess: () => queryClient.invalidateQueries('current-league'),
    }
  );
};
