import axios from 'axios';
import { useMutation, useQueryClient } from 'react-query';
import { useWeek } from '../features/SeasonPicks/hooks/useWeek';
import { useLeague } from '../features/league';

export const useMakePick = () => {
  const queryClient = useQueryClient();
  const { week } = useWeek();
  const league = useLeague();

  const queryKey = [
    'picks',
    league.id,
    league.season,
    week,
  ];

  return useMutation(
    (picks) => {
      return axios.put('/api/pick', picks).then((res) => res.data);
    },
    {
      onMutate: async (picks) => {
        await queryClient.cancelQueries({ queryKey: queryKey })

        // Snapshot the previous value
        const prevGames = queryClient.getQueryData(queryKey);


        // Optimistically update to the new value
        queryClient.setQueryData(queryKey, (old) => {
          return old.map((game )=> {
            if (game.id === picks.gameId) {
              console.log("selectedTeamIds", picks.selectedTeamIds)
              return {
                ...game,
                picks: picks.selectedTeamIds.map((p) => ({ selectedTeamId: p, gameId: game.id })),
              };
            }
            return game
          });
        })
        return { prevGames }
      },
      onError: (err, newTodo, context) => {
        queryClient.setQueryData(queryKey, context.prevGames)
      },
      onSettled: () => {
        queryClient.invalidateQueries(queryKey);
      },
    }
  );
};
