import axios from 'axios';
import { useMutation, useQueryClient } from 'react-query';
import { useAuth } from './useAuth';
import { useWeek } from './useWeek';

export const useMakePick = () => {
  const queryClient = useQueryClient();

  const {
    user: { id },
  } = useAuth();
  const week = useWeek();

  return useMutation(
    (picks) => {
      return axios.put('/api/pick', picks).then((res) => res.data);
    },
    {
      onSettled: () => {
        queryClient.invalidateQueries(['picks', id, week]);
      },
    }
  );
};
