import { useQuery, UseQueryOptions } from 'react-query';
import { RequestErrorType } from '../utils/agent';

export function useLocalQuery<T>(
  queryKey: string,
  queryFnc: () => Promise<T>,
  options?: UseQueryOptions<T, RequestErrorType>
) {
  return useQuery<T, RequestErrorType>(queryKey, queryFnc, {
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
