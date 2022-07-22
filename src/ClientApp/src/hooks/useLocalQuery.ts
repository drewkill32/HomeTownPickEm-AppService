import axios from 'axios';
import { useQuery, UseQueryOptions } from 'react-query';
import { RequestError } from '../utils/agent';

export function useLocalQuery<T>(
  queryKey: string,
  queryFnc: () => Promise<T>,
  options?: UseQueryOptions<T, RequestError>
) {
  return useQuery<T, RequestError>(queryKey, queryFnc, {
    ...options,
    initialData: () => {
      //get from local storage
      var user = localStorage.getItem(queryKey);
      if (user) {
        return JSON.parse(user);
      }
    },
    onSuccess: (data) => {
      if (data) {
        localStorage.setItem(queryKey, JSON.stringify(data));
      } else {
        localStorage.removeItem(queryKey);
      }
    },
    onError: () => {
      localStorage.removeItem(queryKey);
    },
  });
}
