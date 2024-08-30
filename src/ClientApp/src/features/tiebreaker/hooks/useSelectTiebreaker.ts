import { useMutation, useQueryClient } from 'react-query';
import axios from 'axios';

export const useSelectTiebreaker = () => {
  const queryClient = useQueryClient();
  return useMutation(
    (body: { weeklyGameId: string; totalPoints: number }) => {
      return axios.post(`/api/pick/tiebreaker-pick`, body);
    },
    {
      onSettled: async () => {
        await queryClient.invalidateQueries({
          queryKey: ['tiebreaker-pick'],
        });
      },
    },
  );
};
