import axios from 'axios';
import { useQuery } from 'react-query';
import { RequestErrorType, UserLeague } from '../../../models';
import { LeagueKeys } from '../utils/queryKeys';

export const useGetUserLeagues = () => {
  return useQuery<UserLeague[], RequestErrorType>(LeagueKeys.UserLeagues, () =>
    axios.get('/api/league/user').then((res) => res.data)
  );
};
