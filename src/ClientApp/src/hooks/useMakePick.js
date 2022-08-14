import axios from 'axios';
import { useMutation, useQueryClient } from 'react-query';
import { useWeek } from '../features/SeasonPicks/hooks/useWeek';
import { useLeague } from '../features/league';

export const useMakePick = () => {
  const queryClient = useQueryClient();
  const { week } = useWeek();
  const league = useLeague();

  return useMutation(
    (picks) => {
      return axios.put('/api/pick', picks).then((res) => res.data);
    },
    {
      onSettled: () => {
        queryClient.invalidateQueries([
          'picks',
          league.id,
          league.season,
          week,
        ]);
      },
    }
  );
};
