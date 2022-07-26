import axios from 'axios';
import { useQuery } from 'react-query';
import { RequestErrorType, UserLeague } from '../../../models';

export const useGetUserLeagues = () => {
  return useQuery<UserLeague[], RequestErrorType>('user-leagues', () =>
    axios.get('/api/league/user').then((res) => res.data)
  );
};
